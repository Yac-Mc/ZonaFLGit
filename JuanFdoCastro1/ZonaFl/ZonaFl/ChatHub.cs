using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
//using ZonaFl.Entities;
using ZonaFl.Persistence.Entities;
using ZonaFl.Business.SubSystems;
namespace ZonaFl
{
    public class ChatHub : Hub
    {
        public static string emailIDLoaded = "";

        #region Connect
        public void Connect(string userName, string email)
        {
            

            emailIDLoaded = email;
            var id = Context.ConnectionId;
            SChat schat = new SChat();
            schat.Connect(userName, email, Clients,id);
            
            
        }
        #endregion

        #region Disconnect
        public override System.Threading.Tasks.Task  OnDisconnected(bool stopCalled)
        {
        //using (DB_102707_zonaflEntities dc = new DB_102707_zonaflEntities())
        //{



        SChat schat = new SChat();
        return schat.DisConnect(stopCalled, Context,Clients);

       
        }
        #endregion

        #region Send_To_All
        public void SendMessageToAll(string userName, string message)
        {

            SChat schat = new SChat();
            schat.SendMessageToAll(userName, message, Clients);
          
        }
        #endregion

        #region Private_Messages
        public void SendPrivateMessage(string toUserId, string message, string status)
        {

            SChat schat = new SChat();
            schat.SendPrivateMessage(toUserId, message,status,Clients,Context);

            
        }


        public List<PrivateChatMessage> GetPrivateMessage(string fromid, string toid, int take)
        {

            SChat schat = new SChat();
           return  schat.GetPrivateMessage(fromid, toid, take);

            
        }

       
        public List<PrivateChatMessage> GetScrollingChatData(string fromid, string toid, int start = 10, int length = 1)
        {
            SChat schat = new SChat();
            return schat.GetScrollingChatData(fromid, toid, start, length);
        }
        #endregion

      

        
        
        
    }

   

}