using System;
using System.Collections.Generic;
using System.Threading;
using MonitoringTrainingSessions.Lib;

namespace MonitoringTrainingSessions
{
    public class Main
    {
        static readonly public long DiscordAppId = 841535948797509642;
        private Thread? discordThread;
        private DiscordClient? discord;
        private LogViewer log;
        private bool IsShownError;

        public Main()
        {
            this.log = new LogViewer();
            this.log.log("Program - Start", Status.Info);
            this.connectDiscord();
            
            Main.ConnectOrCreateLobbyDiscord(this.discord, this.log, "ffff", 20, 2);
        }

        /// <summary>
        /// Метод для подключения к дискорду
        /// </summary>
        private bool connectDiscord()
        {
            this.log.log("Discord - Подключение", Status.Info);
            try
            {
                this.discord = new DiscordClient(DiscordAppId);
                this.log.log("Discord - Подключен, начало установки событий", Status.Ok);
                
                this.discord.activityManager.OnActivityJoin += onDiscordActivityJoin;
                
                if (this.discordThread == null || (this.discordThread != null && !this.discordThread.IsAlive))
                {
                    this.discordThread = new Thread((this.runDiscordFlow));
                    this.discordThread.Start();
                }
                this.log.log("Discord - События установленны", Status.Ok);

                IsShownError = false;
                return true;
            }
            catch (Exception)
            {
                this.log.log("Discord - Не найден", Status.Error);
                if (!IsShownError)
                {
                    this.log.log("Discord - Информативное сообщение \"Для работы программы, нужно запустить или установить Discord\"", Status.Info);
                    Notification("Для работы программы, нужно запустить или установить Discord");
                }
                IsShownError = true;
                return false;
            }
        }

        /// <summary>
        /// Событие Discord подключение по приглашению
        /// </summary>
        private void onDiscordActivityJoin(string secret)
        {
            this.log.log($"Discord - Начало подключение по приглашению secret={secret}", Status.Info);
            this.discord?.ConnectLobby(secret);
            this.log.log("Discord - Подключенно к лобби по приглашению", Status.Ok);
            
            // Подключение к лобби игры
            var leagueLobbyId = this.discord?.getMetadata("leagueLobbyId");
            this.log.log($"LCU - Начало подключение к лобби из Discord, leagueLobbyId={leagueLobbyId}", Status.Info);
            // Methods.ConnectLobbyLCU(this.lcuConnector, this.log, leagueLobbyId);
        }
        
        /// <summary>
        /// Поток для работы с Discord
        /// </summary>
        private void runDiscordFlow()
        {
            while (true)
            {
                try
                {
                    this.discord?.RunCallbacks();
                }
                catch (Exception)
                {
                    this.log.log("Discord - Потеря подключения", Status.Error);
                    Thread.Sleep(10 * 1000);
                    this.Reload(this.discordThread);
                }
                Thread.Sleep(1000 / 60);
            }
        }

        private void Notification(string text)
        {
            // this.notifyIcon.ShowBalloonTip(3*100,"", text, ToolTipIcon.Info);
        }
        
        /// <summary>
        /// Перезагрузка приложения
        /// </summary>
        void Reload(object? sender)
        {
            this.log.log("Program - Reload", Status.Info);
            var thread = this.discordThread;
            if (thread != null && thread.Equals(sender))
            {
                this.connectDiscord();
            }

            // this.reconnectLCU();
            // checkLobbyLCU();
        }
        
        /// <summary>
        /// Создание или подключение лобби Discord по partyId
        /// </summary>
        public static void ConnectOrCreateLobbyDiscord(DiscordClient? discord, LogViewer log, string partyId, uint capacity, int members)
        {
            discord?.SearchLobbyByMetadata("leagueLobbyId", partyId, (lobby) =>
            {
                log.log("Discord - Лобби найденно, начало подключения", Status.Info);
                discord.ConnectLobby(lobby.Id, lobby.Secret, (lobbyConn) =>
                {
                    log.log($"Discord - Лобби подключен, id={lobbyConn.Id}", Status.Ok);
                    discord.UpdateActivity(members, (int)capacity);
                });
            }, () =>
            {
                log.log("Discord - Лобби не найденно, начало создания", Status.Info);
                DiscordSDK.LobbyType type = DiscordSDK.LobbyType.Public;
                Dictionary<string, string> metadatas = new Dictionary<string, string> {{ "leagueLobbyId", partyId }};
                discord.CreateLobby(capacity, type, metadatas, (lobby) =>
                {
                    log.log($"Discord - Лобби созданно, id={lobby.Id}", Status.Ok);
                    discord.UpdateActivity(members, (int)capacity);
                });
            });
        }
    }
}