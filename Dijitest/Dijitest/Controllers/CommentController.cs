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
    [RoutePrefix("api/comment")]
    public class CommentController : ApiController
    {
        [CacheOutput(ServerTimeSpan = 86400)]
        [HttpGet]
        public HttpResponseMessage GetComment([FromBody]  Get_ArticleResult Articleobj)
        {
            var claims = ClaimsPrincipal.Current.Identities.First().Claims;
            try
            {
                List<GetCommentResult> obj = new List<GetCommentResult>();
                using (DijiArticleDataContext db = new DijiArticleDataContext())
                {
                    obj = db.GetComment(Articleobj.ID).ToList();
                    db.AddLog(Convert.ToInt32(claims.First(x => x.Type == "ID").Value), "Comment", "Get", HttpStatusCode.OK.ToString());
                }
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, obj);
            }
            catch (Exception Ex)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message);
                using (DijiArticleDataContext db = new DijiArticleDataContext())
                {
                    db.AddLog(Convert.ToInt32(claims.First(x => x.Type == "ID").Value), "Comment", "Get",
                        HttpStatusCode.BadRequest.ToString());
                }
            }
        }
        [InvalidateCacheOutput("GetComment")]
        [HttpPost]
        public HttpResponseMessage PostComment([FromBody]  GetCommentResult obj)
        {
            var claims = ClaimsPrincipal.Current.Identities.First().Claims;
            try
            {
                using (DijiArticleDataContext db = new DijiArticleDataContext())
                {
                    db.Set_Comment(obj.ID, Convert.ToInt32(claims.First(x => x.Type == "ID").Value), obj.ArticleID, obj.Comment);
                    db.AddLog(Convert.ToInt32(claims.First(x => x.Type == "ID").Value), "Comment", "Post", HttpStatusCode.OK.ToString());
                }
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, obj);
            }
            catch (Exception Ex)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message);
                using (DijiArticleDataContext db = new DijiArticleDataContext())
                {
                    db.AddLog(Convert.ToInt32(claims.First(x => x.Type == "ID").Value), "Comment", "Post",
                        HttpStatusCode.BadRequest.ToString());
                }
            }
        }
        [InvalidateCacheOutput("GetComment")]
        [HttpDelete]
        public HttpResponseMessage DeleteArticle([FromBody] GetCommentResult obj)
        {
            var claims = ClaimsPrincipal.Current.Identities.First().Claims;
            try
            {
                using (DijiArticleDataContext db = new DijiArticleDataContext())
                {
                    db.Delete_Comment(obj.ID);
                    db.AddLog(Convert.ToInt32(claims.First(x => x.Type == "ID").Value), "Comment", "Delete", HttpStatusCode.OK.ToString());
                }
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, obj.ID + " Deleted");
            }
            catch (Exception Ex)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.BadRequest, Ex.Message);
                using (DijiArticleDataContext db = new DijiArticleDataContext())
                {
                    db.AddLog(Convert.ToInt32(claims.First(x => x.Type == "ID").Value), "Comment", "Delete",
                        "ID : " + obj.ID + "  " + HttpStatusCode.BadRequest.ToString());
                }
            }
        }
    }
}
