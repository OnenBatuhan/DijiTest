using Dijitest.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace Dijitest.Controllers
{
    [Authorize]
    [RoutePrefix("api/article")]
    public class ArticleController : ApiController
    {
        [CacheOutput(ServerTimeSpan = 86400)]
        [HttpGet]
        public HttpResponseMessage GetArticle()
        {

            var claims = ClaimsPrincipal.Current.Identities.First().Claims;
            try
            {
                List<List_ArticleResult> obj = new List<List_ArticleResult>();
                using (DijiArticleDataContext db = new DijiArticleDataContext())
                {
                    obj = db.List_Article().ToList();
                    db.AddLog(Convert.ToInt32(claims.First(x => x.Type == "ID").Value), "Article", "Get", HttpStatusCode.OK.ToString());
                }
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, obj);
            }
            catch (Exception Ex)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message);
                using (DijiArticleDataContext db = new DijiArticleDataContext())
                {
                    db.AddLog(Convert.ToInt32(claims.First(x => x.Type == "ID").Value), "Article", "Get", HttpStatusCode.BadRequest.ToString());
                }
            }
        }
        [InvalidateCacheOutput("GetArticle")]
        [HttpPost]
        public HttpResponseMessage PostArticle([FromBody]  Get_ArticleResult obj)
        {
            var claims = ClaimsPrincipal.Current.Identities.First().Claims;
            try
            {
                using (DijiArticleDataContext db = new DijiArticleDataContext())
                {
                    db.Set_Article(obj.ID, Convert.ToInt32(claims.First(x => x.Type == "ID").Value), obj.ArticleText, obj.ArticleHeader);
                    db.AddLog(Convert.ToInt32(claims.First(x => x.Type == "ID").Value), "Article", "Post", HttpStatusCode.OK.ToString());
                }
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, obj);
            }
            catch (Exception Ex)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message);
                using (DijiArticleDataContext db = new DijiArticleDataContext())
                {
                    db.AddLog(Convert.ToInt32(claims.First(x => x.Type == "ID").Value), "Article", "Post", HttpStatusCode.BadRequest.ToString());
                }
            }
        }
        [InvalidateCacheOutput("GetArticle")]
        [HttpDelete]
        public HttpResponseMessage DeleteArticle([FromBody] Get_ArticleResult obj)
        {
            var claims = ClaimsPrincipal.Current.Identities.First().Claims;
            try
            {
                using (DijiArticleDataContext db = new DijiArticleDataContext())
                {
                    db.Delete_Article(obj.ID);
                    db.AddLog(Convert.ToInt32(claims.First(x => x.Type == "ID").Value), "Article", "Delete", HttpStatusCode.OK.ToString());
                }
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, obj.ID + " Deleted");
            }
            catch (Exception Ex)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message);
                using (DijiArticleDataContext db = new DijiArticleDataContext())
                {
                    db.AddLog(Convert.ToInt32(claims.First(x => x.Type == "ID").Value), "Article", "Delete",
                        "ID : " + obj.ID + "  " + HttpStatusCode.BadRequest.ToString());
                }
            }
        }
    }
}
