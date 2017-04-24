using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Sitecore.MobileSDK.PasswordProvider;
using Sitecore.MobileSDK.API;
using Sitecore.MobileSDK.API.Items;
using SSCExtensions;
using System.Net;
using System.Net.Security;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private string instanceUrl = "http://cms82u1.test24dk1.dk.sitecore.net/";
        public object ActivityLog { get; private set; }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            // await context.PostAsync($"You sent {activity.Text} which was {length} characters bugaga");

            ISitecoreItem item = await this.GetITem(activity.Text);

            if (item == null) {
                await context.PostAsync("item not found");
            } else { 

            string itemOverview = this.PrimitiveHTMLTagsRemove(item["Overview"].RawValue);
            string imagePath = instanceUrl + this.GetImagePathFromImageRawValue(item["Image"].RawValue);

            IMessageActivity reply = activity.CreateReply(itemOverview);
            reply.Attachments = new List<Attachment>();

            reply.Attachments.Add(new Attachment()
            {
                ContentUrl = imagePath,
                ContentType = "image/png",
                Name = item["Title"].RawValue
            });

            await context.PostAsync(reply);
        }
            context.Wait(MessageReceivedAsync);
        }

        private string PrimitiveHTMLTagsRemove(string sourceText)
        {
            return Regex.Replace
              (sourceText, "<.*?>", string.Empty);
        }
        private string GetImagePathFromImageRawValue(string rawValue)
        {
            //<image src="~/media/605015FD900C488AAD0E8F3762F42A43.ashx" mediaid="{605015FD-900C-488A-AD0E-8F3762F42A43}" mediapath="/Images/Imported/8/0/b/6/a_tokyooverviewFEB09" />
            //<image mediaid="{903A0864-B02F-44CD-9DEB-45CAF0FE08A3}" mediapath="/Images/Planes/14061" src="~/media/903A0864B02F44CD9DEB45CAF0FE08A3.ashx" />
            //<image mediaid="{605015FD-900C-488A-AD0E-8F3762F42A43}"/>

            string prefixToSearch = "mediaid=\"";
            int idLenght = 38;

            string pathPrefix = "~/media/";
            string pathPostfix = ".ashx";

            int srcIndex = rawValue.IndexOf(prefixToSearch) + prefixToSearch.Length;
            int finishIndex = srcIndex + idLenght;
            int mediaPathLenght = finishIndex - srcIndex;

            string mediaId = rawValue.Substring(srcIndex, mediaPathLenght);
            mediaId = mediaId.Replace("{", "");
            mediaId = mediaId.Replace("}", "");
            mediaId = mediaId.Replace("-", "");

            return pathPrefix + mediaId + pathPostfix;
        }

        private async Task<ISitecoreItem> GetITem(string someText)
        {
            try
            {
                //Change SSL checks so that all checks pass
                ServicePointManager.ServerCertificateValidationCallback =
                   new RemoteCertificateValidationCallback(
                        delegate
                        { return true; }
                    );
            }
            catch (Exception ex)
            {
                return null;
            }

            using
     (
       var credentials =
         new ScUnsecuredCredentialsProvider(
           "admin",
           "b",
           "sitecore"
         )
     )
            {

                using (
                    var session = SitecoreSSCSessionBuilder.AuthenticatedSessionWithHost(instanceUrl)
                                                            .Credentials(credentials)
                                                            .DefaultDatabase("web")
                                                            .BuildSession()
                       )
                {
                    var ext = ExtendedSessionBuilder.ExtendedSessionWith(session)
                        .PathForTemporaryItems("/sitecore/content/Home")
                                             .Build();


                    var request = ExtendedSSCRequestBuilder.SitecoreQueryRequest("/sitecore/content/Home//*[@@templatename='City Item' and contains(@Title, '"+ someText + "')]")
                                                           .Build();


                    var response = await ext.SearchBySitecoreQueryAsync(request);

                    if (response == null || response.ResultCount == 0)
                    {
                        return null;
                    }

                    return response[0];
                }
            }
        }   

    }
}