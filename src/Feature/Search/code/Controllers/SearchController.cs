using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using BlogHelix.Feature.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Solr_Search_Application.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        [HttpGet]
        public ActionResult SearchForm()
        {
            return PartialView("/Views/Search/_SearchFormPartial.cshtml");
        }
        [HttpPost]
        public ActionResult searchPredicate(FormCollection form)
        {
            string query = form["searchInput"];
            List<Sitecore.Data.Items.Item> blogItems = new List<Sitecore.Data.Items.Item>();
            List<BlogCard> BlogCardsCollection = new List<BlogCard>();
            var rootitem = Sitecore.Context.Database.GetItem("{F662ECAB-1E03-439A-B62E-D9B8B52B9BF9}");
            var Websitesettings = Sitecore.Context.Database.GetItem("{E90A3045-AA41-4E3E-AE05-67DB21ABFD27}");

            blogItems = rootitem.Axes.GetDescendants().ToList();

            foreach (Sitecore.Data.Items.Item items in blogItems.ToList())
            {
                if (items.TemplateID.ToString() != "{252B796D-F49D-4574-92DD-219C0A948C85}")
                {
                    blogItems.Remove(items);
                }
            }

            for (int i = 0; i < blogItems.Count; i++)
            {
                BlogCard BlogModel = new BlogCard();
                var imageUrl = string.Empty;

                Sitecore.Data.Fields.ImageField imageField = blogItems[i].Fields["BlogSmallImage"];
                if (imageField?.MediaItem != null)
                {
                    var image = new MediaItem(imageField.MediaItem);
                    imageUrl = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(image));
                    BlogModel.BlogSImage = imageUrl;
                }
                BlogModel.Category = blogItems[i].Fields["Category"].Value;
                BlogModel.BlogTitle = blogItems[i].Fields["BlogCardTitle"].Value;

                Sitecore.Data.Fields.DateField dateTimeField = blogItems[i].Fields["PostedDate"];

                string dateTimeString = dateTimeField.Value;

                DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                BlogModel.date = dateTimeStruct.ToShortDateString();

                BlogModel.ShortDesc = blogItems[i].Fields["ShortDescription"].Value;
                BlogModel.LongDesc = blogItems[i].Fields["LongDescription"].Value;
                BlogModel.Readonbtn = Websitesettings.Fields["BlogCardButtonText"].Value;
                BlogModel.sitecoreItem = blogItems[i];
                BlogModel.BlogURL = Sitecore.Links.LinkManager.GetItemUrl(blogItems[i]);


                BlogCardsCollection.Add(BlogModel);
            }

            if (query is null)
                query = "hi";

            List<BlogCard> results = BlogCardsCollection.FindAll(Findtitle);



            bool Findtitle(BlogCard bk)
            {

                if (bk.BlogTitle.Contains(query) || bk.Category.Contains(query) || bk.ShortDesc.Contains(query) || bk.LongDesc.Contains(query))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            ViewBag.searchCards = results;
            return View("/Views/Search/SearchCards.cshtml", ViewBag.searchCards);
        }
    }
}