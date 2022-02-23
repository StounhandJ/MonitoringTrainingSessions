using System;
using System.Collections.Generic;
using DiscordSDK;

namespace MonitoringTrainingSessions.Lib
{
    public class DiscordClient
    {
        private Discord client;
        public ActivityManager activityManager;
        private LobbyManager lobbyManager;
        private VoiceManager voiceManager;
        private UserManager userManager;
        private Lobby? currentLobby;


        public DiscordClient(long clientId)
        {
            client = new Discord(clientId, (UInt64) CreateFlags.NoRequireDiscord);
            client.SetLogHook(LogLevel.Debug, logHook);

            activityManager = client.GetActivityManager();
            lobbyManager = client.GetLobbyManager();
            voiceManager = client.GetVoiceManager();
            userManager = client.GetUserManager();
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

        public void ConnectLobby(string activitySecret, Action<Lobby> okCallback = null, Action<Result> errorCallback = null)
        {
            lobbyManager.ConnectLobbyWithActivitySecret(activitySecret, (Result result, ref Lobby lobby) =>
            {
                if (result==Result.Ok)
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
        
        public void ConnectLobby(long lobbyId, string lobbySecret, Action<Lobby> okCallback = null, Action<Result> errorCallback = null)
        {
            lobbyManager.ConnectLobby(lobbyId, lobbySecret,(Result result, ref Lobby lobby) =>
            {
                if (result==Result.Ok)
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
            if (currentLobby==null) return;
            User currentUser = userManager.GetCurrentUser();
            int membersCount = lobbyManager.MemberCount(currentLobby.Value.Id);
            if (IsUserOwner() && membersCount>1)
            {
                for (int i = 0; i < membersCount; i++)
                {
                    var user = lobbyManager.GetMemberUserId(currentLobby.Value.Id, i);
                    if (currentUser.Id!=user)
                    {
                        var txn = lobbyManager.GetLobbyUpdateTransaction(currentLobby.Value.Id);
                        txn.SetOwner(user);
                        lobbyManager.UpdateLobby(currentLobby.Value.Id, txn, result => {});
                        break;
                    }
                }
                DisconnectVoice();
                DisconnectLobby();
            }
            else if (IsUserOwner())
            {
                DeleteLobby();
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
            if (currentLobby==null) return;
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
    }
}