using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Sitecore.MobileSDK.API.Items;
using System.Collections.Generic;
using SCHelpers;

namespace Bot_Application1.Dialogs
{
    [Serializable]

    public class RootDialog : IDialog<object>
    {
        private ScNetworkHelper network = new ScNetworkHelper();
        public object ActivityLog { get; private set; }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            StringComparison ignoreCase = StringComparison.OrdinalIgnoreCase;

            IMessageActivity reply = null;

            //refactor this
            if (activity.Text.IndexOf("all cities", ignoreCase) >= 0)
            {
                reply = await this.GetReplyAllCities(activity);
            }
            else {
                if (activity.Text.IndexOf("all regions", ignoreCase) >= 0)
                {
                    reply = await this.GetReplyAllRegions(activity);
                }
                else
                {
                    if (activity.Text.IndexOf("all countries", ignoreCase) >= 0)
                    {
                        reply = await this.GetReplyAllCountries(activity);
                    }
                    else
                    {
                        if (activity.Text.IndexOf("countries for", ignoreCase) >= 0)
                        {
                            reply = await this.GetReplyCountriesForRegion(Helper.LastWord(activity.Text), activity);
                        }
                        else
                        {
                            if (activity.Text.IndexOf("cities for", ignoreCase) >= 0)
                            {
                                reply = await this.GetReplyCitiesForCountry(Helper.LastWord(activity.Text), activity);
                            }
                            else
                            {
                                reply = await this.GetReplyForCityNamed(activity.Text, activity);
                            }
                        }
                    }
                }
                
            }

            await context.PostAsync(reply);
           

            context.Wait(MessageReceivedAsync);
        }  

        private async Task<IMessageActivity> GetReplyForCityNamed(string name, Activity activity) {

            ISitecoreItem item = await network.GetCityNamed(activity.Text);

            IMessageActivity reply = null;

            if (item == null)
            {
                reply = activity.CreateReply("item not found");
             }
            else
            {

                string itemOverview = Helper.PrimitiveHTMLTagsRemove(item["Overview"].RawValue);
                string itemTitle = item["Title"].RawValue;
                string imagePath = ScNetworkSettings.instanceUrl + Helper.GetImagePathFromImageRawValue(item["Image"].RawValue);

                string replyText = "### " + itemTitle + "\n\n***\n\n" + itemOverview + "\n\n***\n\n";

                reply = activity.CreateReply(replyText);
                reply.Attachments = new List<Attachment>();

                reply.Attachments.Add(new Attachment()
                {
                    ContentUrl = imagePath,
                    ContentType = "image/png",
                    Name = itemTitle
                });
            }

            return reply;
        }

        private IMessageActivity ProceedItemsToList(ScItemsResponse items, string errorText, Activity activity)
        {
            IMessageActivity reply = null;

            if (items == null)
            {
                reply = activity.CreateReply(errorText);
            }
            else
            {
                string replyText = "";
                foreach (ISitecoreItem elem in items)
                {
                    replyText = replyText + elem["Title"].RawValue + "\n\n";
                }

                reply = activity.CreateReply(replyText);
            }

            return reply;
        }

        private async Task<IMessageActivity> GetReplyCountriesForRegion(string regionTitle, Activity activity)
        {
            ScItemsResponse countries = await network.GetCountriesForRegion(regionTitle);

            return this.ProceedItemsToList(countries, "Countries not found", activity);
        }

        private async Task<IMessageActivity> GetReplyCitiesForCountry(string countryTitle, Activity activity)
        {
            ScItemsResponse cities = await network.GetCitiesForCountry(countryTitle);

            return this.ProceedItemsToList(cities, "Cities not found", activity);
        }

        private async Task<IMessageActivity> GetReplyAllCities(Activity activity)
        {
            ScItemsResponse cities = await network.GetAllCities();

            return this.ProceedItemsToList(cities, "Cities not found", activity);
        }

        private async Task<IMessageActivity> GetReplyAllCountries(Activity activity)
        {
            ScItemsResponse countries = await network.GetAllCountries();

            return this.ProceedItemsToList(countries, "Countries not found", activity);
        }

        private async Task<IMessageActivity> GetReplyAllRegions(Activity activity)
        {
            ScItemsResponse regions = await network.GetAllRegions();

            return this.ProceedItemsToList(regions, "Regions not found", activity);
        }

    }
}