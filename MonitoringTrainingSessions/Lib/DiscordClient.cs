using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using MonitoringTrainingSessions.Models;
using User = Discord.User;

namespace MonitoringTrainingSessions.Lib
{
    public class DiscordClient
    {
        private Discord.Discord client;
        public ActivityManager activityManager;
        private LobbyManager lobbyManager;
        private VoiceManager voiceManager;
        private UserManager userManager;
        private Lobby? currentLobby;
        private string errorMessage;
        private Thread? discordThread;
        private long clientId;
        private uint capacity;

        public delegate void ActivityJoinHandler(Lobby currentLobby);
        public delegate void MemberUpdateHandler(int count);

        public event ActivityJoinHandler OnActivityJoin;
        
        public event MemberUpdateHandler OnMemberUpdate;


        public DiscordClient(long clientId)
        {
            this.clientId = clientId;
            if (!this.connectDiscord())
                return;

            client.SetLogHook(LogLevel.Debug, logHook);

            activityManager = client.GetActivityManager();
            lobbyManager = client.GetLobbyManager();
            voiceManager = client.GetVoiceManager();
            userManager = client.GetUserManager();
            lobbyManager.OnMemberUpdate += (lobbyId, userId) =>
            {
                int count = lobbyManager.MemberCount(lobbyId);
                this.UpdateActivity(count, (int)capacity);
                OnMemberUpdate?.Invoke(count);
            };
            this.activityManager.OnActivityJoin += (secret) =>
            {
                App.LogViewer.log($"Discord - Начало подключение по приглашению secret={secret}", Status.Info);
                // this.ConnectLobby(secret);
                // App.LogViewer.log("Discord - Подключенно к лобби по приглашению", Status.Ok);
                // if (currentLobby != null)
                // {
                    OnActivityJoin?.Invoke(currentLobby ?? new Lobby());
                // }
            };
        }

        private bool connectDiscord()
        {
            App.LogViewer.log("Discord - Подключение", Status.Info);
            try
            {
                client = new Discord.Discord(this.clientId, (UInt64)CreateFlags.NoRequireDiscord);
                App.LogViewer.log("Discord - Подключен, начало установки событий", Status.Ok);

                if (this.discordThread == null || (this.discordThread != null && !this.discordThread.IsAlive))
                {
                    this.discordThread = new Thread((this.runDiscordFlow));
                    this.discordThread.Start();
                }

                App.LogViewer.log("Discord - События установленны", Status.Ok);

                errorMessage = "";
                return true;
            }
            catch (Exception)
            {
                App.LogViewer.log("Discord - Не найден", Status.Error);
                errorMessage = "Для работы программы, нужно запустить или установить Discord";
                return false;
            }
        }


        public bool IsUserOwner()
        {
            var lobby = currentLobby;
            return lobby != null && lobby.Value.OwnerId == userManager.GetCurrentUser().Id;
        }

        public bool IsConnectLobby()
        {
            return currentLobby.HasValue;
        }

        public bool IsMute()
        {
            return voiceManager.IsSelfMute();
        }

        /// <summary>
        /// Возвращает всех учатсников лобби, кроме вас
        /// </summary>
        public List<User> GetMembers()
        {
            if (currentLobby == null) return new List<User>();

            List<User> members = new List<User>();
            User currentUser = userManager.GetCurrentUser();
            foreach (var user in lobbyManager.GetMemberUsers(currentLobby.Value.Id))
            {
                if (currentUser.Id != user.Id)
                {
                    members.Add(user);
                }
            }

            return members;
        }

        public string getMetadata(string key)
        {
            if (currentLobby != null)
                return lobbyManager.GetLobbyMetadataValue(currentLobby.Value.Id, key);
            return null;
        }

        public void SetLocalVolume(long userID, byte value)
        {
            voiceManager.SetLocalVolume(userID, value);
        }

        public int GetLocalVolume(long userID)
        {
            return voiceManager.GetLocalVolume(userID);
        }

        public void ConnectOrCreateLobbyDiscord(string partyId, uint capacity)
        {
            this.capacity = capacity;
            this.SearchLobbyByMetadata("monLobbyId", partyId, (lobby) =>
            {
                App.LogViewer.log("Discord - Лобби найденно, начало подключения", Status.Info);
                this.ConnectLobby(lobby.Id, lobby.Secret, (lobbyConn) =>
                {
                    App.LogViewer.log($"Discord - Лобби подключен, id={lobbyConn.Id}", Status.Ok);
                    this.UpdateActivity(lobbyManager.MemberCount(lobby.Id), (int)this.capacity);
                });
            }, () =>
            {
                App.LogViewer.log("Discord - Лобби не найденно, начало создания", Status.Info);
                Dictionary<string, string> metadatas = new Dictionary<string, string> { { "monLobbyId", partyId } };
                this.CreateLobby(capacity, LobbyType.Public, metadatas, (lobby) =>
                {
                    App.LogViewer.log($"Discord - Лобби созданно, id={lobby.Id}", Status.Ok);
                    this.UpdateActivity(lobbyManager.MemberCount(lobby.Id), (int)this.capacity);
                });
            });
        }


        public void CreateLobby(uint lobbyCapacity, LobbyType lobbyType, Dictionary<string, string> metadatas,
            Action<Lobby> okCallback = null, Action<Result> errorCallback = null)
        {
            var txn = lobbyManager.GetLobbyCreateTransaction();
            txn.SetCapacity(lobbyCapacity);
            txn.SetType(lobbyType);
            foreach (var var in metadatas)
            {
                txn.SetMetadata(var.Key, var.Value);
            }

            lobbyManager.CreateLobby(txn, (Result result, ref Lobby lobby) =>
            {
                if (result == Result.Ok)
                {
                    currentLobby = lobby;
                    ConnectVoice(lobby);
                    okCallback?.Invoke(lobby);
                }
                else
                {
                    errorCallback?.Invoke(result);
                }
            });
        }

        public void DeleteLobby()
        {
            if (currentLobby != null)
                lobbyManager.DeleteLobby(currentLobby.Value.Id,
                    res => Console.WriteLine("Successfully deleted"));
            currentLobby = null;
        }

        /// <summary>
        /// Поиск лобби по Metadata.
        /// </summary>
        /// <param name="key">Название Metadata</param>
        /// <param name="value">Значение Metadata</param>
        /// <param name="okCallback"></param>
        /// <param name="errorCallback"></param>
        public void SearchLobbyByMetadata(string key, string value, Action<Lobby> okCallback, Action errorCallback)
        {
            var query = lobbyManager.GetSearchQuery();
            query.Filter($"metadata.{key}", LobbySearchComparison.Equal, LobbySearchCast.String, value);
            query.Limit(1);
            lobbyManager.Search(query, _ =>
            {
                if (lobbyManager.LobbyCount() == 1)
                {
                    okCallback(lobbyManager.GetLobby(lobbyManager.GetLobbyId(0)));
                }
                else
                {
                    errorCallback();
                }
            });
        }

        public void ConnectLobby(string activitySecret, Action<Lobby> okCallback = null,
            Action<Result> errorCallback = null)
        {
            lobbyManager.ConnectLobbyWithActivitySecret(activitySecret, (Result result, ref Lobby lobby) =>
            {
                if (result == Result.Ok)
                {
                    currentLobby = lobby;
                    ConnectVoice(lobby);
                    okCallback?.Invoke(lobby);
                }
                else
                {
                    errorCallback?.Invoke(result);
                }
            });
        }

        public void ConnectLobby(long lobbyId, string lobbySecret, Action<Lobby> okCallback = null,
            Action<Result> errorCallback = null)
        {
            lobbyManager.ConnectLobby(lobbyId, lobbySecret, (Result result, ref Lobby lobby) =>
            {
                if (result == Result.Ok)
                {
                    currentLobby = lobby;
                    ConnectVoice(lobby);
                    okCallback?.Invoke(lobby);
                }
                else
                {
                    errorCallback?.Invoke(result);
                }
            });
        }

        /// <summary>
        /// Полное отключение от лобби с возможностью его удаления.
        /// </summary>
        public void LobbySmartDisconnect()
        {
            if (currentLobby == null) return;
            User currentUser = userManager.GetCurrentUser();
            int membersCount = lobbyManager.MemberCount(currentLobby.Value.Id);
            DisconnectVoice();
            if (IsUserOwner() && membersCount > 1)
            {
                for (int i = 0; i < membersCount; i++)
                {
                    var user = lobbyManager.GetMemberUserId(currentLobby.Value.Id, i);
                    if (currentUser.Id != user)
                    {
                        var txn = lobbyManager.GetLobbyUpdateTransaction(currentLobby.Value.Id);
                        txn.SetOwner(user);
                        lobbyManager.UpdateLobby(currentLobby.Value.Id, txn, result => { });
                        break;
                    }
                }

                DisconnectLobby();
            }
            else if (IsUserOwner())
            {
                DeleteLobby();
            }
            else
            {
                DisconnectLobby();
            }

            ClearActivity();
        }

        public void DisconnectLobby()
        {
            if (currentLobby == null) return;
            lobbyManager.DisconnectLobby(currentLobby.Value.Id,
                res => Console.WriteLine("Successfully disconnected from lobby"));
            currentLobby = null;
        }

        public void ConnectVoice(Lobby lobby)
        {
            lobbyManager.ConnectVoice(lobby.Id, res => Console.WriteLine("Connected to lobby voice"));
            // this.UpdateActivity(lobby);
        }

        public void DisconnectVoice()
        {
            if (currentLobby != null)
                lobbyManager.DisconnectVoice(currentLobby.Value.Id,
                    res => Console.WriteLine("Successfully disconnected from voice"));
        }

        public void Mute()
        {
            if (voiceManager.IsSelfMute())
            {
                voiceManager.SetSelfMute(false);
            }
            else
            {
                voiceManager.SetSelfMute(true);
            }
        }

        public void UpdateActivity(int CurrentSize, int MaxSize)
        {
            if (currentLobby == null) return;
            var lobby = currentLobby.Value;
            var activity = new Activity
            {
                State = "Testing things",
                Party =
                {
                    Id = lobby.Id.ToString(),
                    Size =
                    {
                        CurrentSize = CurrentSize,
                        MaxSize = MaxSize,
                    },
                },
                Secrets =
                {
                    Join = lobbyManager.GetLobbyActivitySecret(lobby.Id),
                },
                Instance = true,
            };

            activityManager.UpdateActivity(activity, result => { Console.WriteLine("Update Activity {0}", result); });
        }

        public void ClearActivity()
        {
            activityManager.ClearActivity(res => Console.WriteLine("Successfully cleared activity"));
        }

        private void logHook(LogLevel level, string message)
        {
            Console.WriteLine("Log[{0}] {1}", level, message);
        }

        public void RunCallbacks()
        {
            client.RunCallbacks();
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public void close()
        {
            this.LobbySmartDisconnect();
            // this.cancelTokenSource?.Cancel();
            // this.discordTask = null;
            // this.connectDiscord();
        }

        private void runDiscordFlow()
        {
            while (true)
            {
                try
                {
                    this.RunCallbacks();
                }
                catch (Exception)
                {
                    App.LogViewer.log("Discord - Потеря подключения", Status.Error);
                    Thread.Sleep(10 * 1000);
                    this.connectDiscord();
                }

                Thread.Sleep(1000 / 60);
            }
        }
    }
}