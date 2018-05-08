using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Persistence;

using ZonaFl.Persistence.Repository;
using Omu.ValueInjecter;
using ZonaFl.Persistence.Entities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ZonaFl.Business.SubSystems
{
    public class SChat:Hub
    {
        public static string emailIDLoaded = "";
        public SChat()
        {

            //using (var db = new ZonaFlContext())
            //{
            //    try
            //    {

            //        db.SaveChanges();
            //        db.Categories.Add(new ZonaFl.Persistence.Entities.Category {  Name = "Páginas web y software" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Aplicaciones para Móviles" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Diseño" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Producción Multimedia" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Animación 3D" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Office" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Traducción y Redacción" });
            //        //db.Categories.Add(new ZonaFl.Persistence.Entities.Category { Id = 0, Name = "Ingeniería y arquitectura" });
            //    }

            //    catch (Exception er)
            //    {


            //    }

                
            //}
            //  //  ZonaFlContext db = new ZonaFlContext();
           
        }

        

        public List<PrivateChatMessage> GetPrivateMessage(string fromid, string toid, int take)
        {

            ChatPrivateMessageMasterRepository<ChatPrivateMessageMaster> chpmm = new ChatPrivateMessageMasterRepository<ChatPrivateMessageMaster>();
            ChatPrivateMessageDetailsRepository<ChatPrivateMessageDetails> chpmd = new ChatPrivateMessageDetailsRepository<ChatPrivateMessageDetails>();

            //using (Surajit_TestEntities dc = new Surajit_TestEntities())
            //{
            List<PrivateChatMessage> msg = new List<PrivateChatMessage>();

                var v = (from a in chpmm.GetList().ToList()
                         join b in chpmd.GetList().ToList() on a.EmailID equals b.MasterEmailID into cc
                         from c in cc
                         where (c.MasterEmailID.Equals(fromid) && c.ChatToEmailID.Equals(toid)) || (c.MasterEmailID.Equals(toid) && c.ChatToEmailID.Equals(fromid))
                         orderby c.Id descending
                         select new
                         {
                             UserName = a.UserName,
                             Message = c.Message,
                             ID = c.Id
                         }).Take(take).ToList();
                v = v.OrderBy(s => s.ID).ToList();

                foreach (var a in v)
                {
                    var res = new PrivateChatMessage()
                    {
                        userName = a.UserName,
                        message = a.Message
                    };
                    msg.Add(res);
                }
                return msg;
            //}
        }

        public void SendMessageToAll(string userName, string message, Microsoft.AspNet.SignalR.Hubs.IHubCallerConnectionContext<dynamic> Clients2)
        {
            // store last 100 messages in cache
            AddAllMessageinCache(userName, message);

            // Broad cast message
            Clients2.All.messageReceived(userName, message);
        }

        #region Private_Messages
        public void SendPrivateMessage(string toUserId, string message, string status, Microsoft.AspNet.SignalR.Hubs.IHubCallerConnectionContext<dynamic> Clients2, HubCallerContext Context2)
        {
            string fromUserId = Context2.ConnectionId;
            ChatUserDetailRepository<ChatUserDetail> chud = new ChatUserDetailRepository<ChatUserDetail>();
            //using (Surajit_TestEntities dc = new Surajit_TestEntities())
            //{
            var toUser = chud.GetList(new { ConnectionId = toUserId }).FirstOrDefault();// ChatUserDetails.FirstOrDefault(x => x.ConnectionId == toUserId);
                var fromUser = chud.GetList(new { ConnectionId = fromUserId }).FirstOrDefault(); //dc.ChatUserDetails.FirstOrDefault(x => x.ConnectionId == fromUserId);
            if (toUser != null && fromUser != null)
                {
                    if (status == "Click")
                        AddPrivateMessageinCache(fromUser.EmailID, toUser.EmailID, fromUser.UserName, message);

                    // send to 
                    Clients2.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message, fromUser.EmailID, toUser.EmailID, status, fromUserId);

                    // send to caller user
                    Clients2.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message, fromUser.EmailID, toUser.EmailID, status, fromUserId);
                }
            //}
        }
        #endregion
        #region Save_Cache
        private void AddAllMessageinCache(string userName, string message)
        {
            ChatMessageDetailRepository<ChatMessageDetail> chatmd = new ChatMessageDetailRepository<ChatMessageDetail>();
            //using (DB_102707_zonaflEntities dc = new DB_102707_zonaflEntities())
            //{
                var messageDetail = new ChatMessageDetail
                {
                    UserName = userName,
                    Message = message,
                    EmailID = emailIDLoaded
                };
            chatmd.Insert(messageDetail);
                //dc.ChatMessageDetails.Add(messageDetail);
                //dc.SaveChanges();
            //}
        }
        #endregion


        private void AddPrivateMessageinCache(string fromEmail, string chatToEmail, string userName, string message)
        {
            ChatPrivateMessageMasterRepository<ChatPrivateMessageMaster> chmmr = new ChatPrivateMessageMasterRepository<ChatPrivateMessageMaster>();
            //using (Surajit_TestEntities dc = new Surajit_TestEntities())
            //{
            // Save master
            var master = chmmr.GetList(new { EmailID = fromEmail }).ToList();//dc.ChatPrivateMessageMasters.ToList().Where(a => a.EmailID.Equals(fromEmail)).ToList();
                if (master.Count == 0)
                {
                    var result = new ChatPrivateMessageMaster
                    {
                        EmailID = fromEmail,
                        UserName = userName
                    };
                chmmr.Insert(result);// dc.ChatPrivateMessageMasters.Add(result);
                    //dc.SaveChanges();
                }

                // Save details
                var resultDetails = new ChatPrivateMessageDetails
                {
                    MasterEmailID = fromEmail,
                    ChatToEmailID = chatToEmail,
                    Message = message
                };

            ChatPrivateMessageDetailsRepository<ChatPrivateMessageDetails> chpmd = new ChatPrivateMessageDetailsRepository<ChatPrivateMessageDetails>();
            chpmd.Insert(resultDetails);// dc.ChatPrivateMessageDetails.Add(resultDetails);
               // dc.SaveChanges();
           // }
        }

        public void Connect(string userName, string email, Microsoft.AspNet.SignalR.Hubs.IHubCallerConnectionContext<dynamic> Clients2, string id)
        {
            emailIDLoaded = email;
            //var id = Context.ConnectionId;
            //using (DB_102707_zonaflEntities dc = new DB_102707_zonaflEntities())
            //{
            ChatUserDetailRepository<ChatUserDetail> chatud = new ChatUserDetailRepository<ChatUserDetail>();
            OfferRepository<Offer> offerrepo = new OfferRepository<Offer>();
            OfferUserRepository<OfferUser> offeurrepo = new OfferUserRepository<OfferUser>();
            OfferUserRepository<AspNetUsers> userrepo = new OfferUserRepository<AspNetUsers>();
           ChatUserDetail item =chatud.GetList(new { EmailID = email }).FirstOrDefault();
          
            //ChatUserDetail item=chatud.GetList("where EmailID='" + email+"'").FirstOrDefault();
            //var item = dc.ChatUserDetails.FirstOrDefault(x => x.EmailID == email);
            if (item != null)
            {
                chatud.Delete(item.Id);
                //dc.ChatUserDetails.Remove(item);
                //dc.SaveChanges();

                // Disconnect
                Clients2.All.onUserDisconnectedExisting(item.ConnectionId, item.UserName);
            }
           


            AspNetUsers user= userrepo.GtUserByEmail(email);
            if (chatud.GetList(new { EmailId = user.Email }).FirstOrDefault() == null)
            {
                var userdetails = new ChatUserDetail
                {
                    ConnectionId = id,
                    UserName = userName,
                    EmailID = email
                };
                chatud.Insert(userdetails);
            }

            List<ChatUserDetail> Users = new List<ChatUserDetail>();
            if (!user.Freelance)
            {
                List<Offer> offertsuser = offerrepo.GetOffertsByContractorEmail(email);
                string valueoffer = string.Join(",", offertsuser.Select(e => e.Id).ToArray());
                Users = chatud.GetUsersByOfferContractorid(valueoffer);
            }
            else
            {
                List<OfferUser> offertsuser = offeurrepo.GetOffersUser(user.Id);
                string valueoffer = string.Join(",", offertsuser.Select(e => e.IdOffer).ToArray());
                Users = chatud.GetContractorsByOffers(valueoffer);
            }

            //var Users = chatud.GetList().ToList();
            if (Users.Where(x => x.EmailID == email).ToList().Count == 0)
            {
                //var userdetails = new ChatUserDetail
                //{
                //    ConnectionId = id,
                //    UserName = userName,
                //    EmailID = email
                //};
                //chatud.Insert(userdetails);
                //dc.SaveChanges();

                // send to caller
                var connectedUsers = chatud.GetList();
                ChatMessageDetailRepository<ChatMessageDetail> chatmd = new ChatMessageDetailRepository<ChatMessageDetail>();
                var CurrentMessage = chatmd.GetList();//dc.ChatMessageDetails.ToList();
                Clients2.Caller.onConnected(id, userName, connectedUsers, CurrentMessage);
                // }

                // send to all except caller client
                Clients2.AllExcept(id).onNewUserConnected(id, userName, email);
            }
        }

        #region Disconnect
        public  System.Threading.Tasks.Task DisConnect(bool stopCalled, HubCallerContext Context2, Microsoft.AspNet.SignalR.Hubs.IHubCallerConnectionContext<dynamic> Clients2)
        {
            ChatUserDetailRepository<ChatUserDetail> chatud = new ChatUserDetailRepository<ChatUserDetail>();
            //using (Surajit_TestEntities dc = new Surajit_TestEntities())
            //{
            var item = chatud.GetList(new { ConnectionId = Context2.ConnectionId }).FirstOrDefault();
                //var item = dc.ChatUserDetails.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
                if (item != null)
                {
                chatud.Delete(item.Id); //dc.ChatUserDetails.Remove(item);
                    //dc.SaveChanges();

                    var id = Context2.ConnectionId;
                    Clients2.All.onUserDisconnected(id, item.UserName);
                }
            //}
            return base.OnDisconnected(stopCalled);
        }
        #endregion


        private int takeCounter = 0;
        private int skipCounter = 0;
        public List<PrivateChatMessage> GetScrollingChatData(string fromid, string toid, int start = 10, int length = 1)
        {
            takeCounter = (length * start); // 20
            skipCounter = ((length - 1) * start); // 10
            ChatPrivateMessageMasterRepository<ChatPrivateMessageMaster> chpmm = new ChatPrivateMessageMasterRepository<ChatPrivateMessageMaster>();
            ChatPrivateMessageDetailsRepository<ChatPrivateMessageDetails> chpmd = new ChatPrivateMessageDetailsRepository<ChatPrivateMessageDetails>();
            
            //using (Surajit_TestEntities dc = new Surajit_TestEntities())
            //{
                List<PrivateChatMessage> msg = new List<PrivateChatMessage>();
                var v = (from a in chpmm.GetList().ToList()
                         join b in chpmd.GetList().ToList() on a.EmailID equals b.MasterEmailID into cc
                         from c in cc
                         where (c.MasterEmailID.Equals(fromid) && c.ChatToEmailID.Equals(toid)) || (c.MasterEmailID.Equals(toid) && c.ChatToEmailID.Equals(fromid))
                         orderby c.Id descending
                         select new
                         {
                             UserName = a.UserName,
                             Message = c.Message,
                             ID = c.Id
                         }).Take(takeCounter).Skip(skipCounter).ToList();

                foreach (var a in v)
                {
                    var res = new PrivateChatMessage()
                    {
                        userName = a.UserName,
                        message = a.Message
                    };
                    msg.Add(res);
                }
                return msg;
           // }
        }


       



       

       
    }

    
}
