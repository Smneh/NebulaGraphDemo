using NebulaGraphDemo.Models;
using Newtonsoft.Json;

namespace NebulaGraphDemo.Extensions;

public static class Mapper
{
    public static Dto.Post ToDto(this Models.Post post)
    {
        return new Dto.Post
        {
            IssuerId = post.IssuerId,
            IssuerType = post.IssuerType,
            RegDate = post.RegDate,
            RegTime = post.RegTime,
            IsShareable = post.IsShareable,
            IsCommentable = post.IsCommentable,
            IsPublic = post.IsPublic,
            PostType = post.PostType,
            RegUser = post.RegUser,
            UniqueId = post.UniqueId,
            RegDateTime = post.RegDateTime,
            LikeCount = post.LikeCount,
            CommentCount = post.CommentCount,
            ViewCount = post.ViewCount,
            Content = post.Content,
            Attachments = JsonConvert.SerializeObject(post.Attachments),
            IssuerPostId = post.IssuerPostId,
            IsSurvey = post.IsSurvey,
            PostTypeId = post.PostTypeId,
            ParentIssuerType = post.ParentIssuerType,
            SurveyId = post.SurveyId,
            ShareCount = post.ShareCount,
            Uuid = post.Uuid,
            ContentUuid = post.ContentUuid,
            ParentUuid = post.ParentUuid,
            Edited = post.Edited,
            EditDateTime = post.EditDateTime,
        };
    }

    public static Models.Post ToEntity(this Dto.Post post)
    {
        var postEntity = new Models.Post
        {
            IssuerId = post.IssuerId,
            IssuerType = post.IssuerType,
            RegDate = post.RegDate,
            RegTime = post.RegTime,
            IsShareable = post.IsShareable,
            IsCommentable = post.IsCommentable,
            IsPublic = post.IsPublic,
            PostType = post.PostType,
            RegUser = post.RegUser,
            UniqueId = post.UniqueId,
            RegDateTime = post.RegDateTime,
            LikeCount = post.LikeCount,
            CommentCount = post.CommentCount,
            ViewCount = post.ViewCount,
            Content = post.Content,
            IssuerPostId = post.IssuerPostId,
            IsSurvey = post.IsSurvey,
            PostTypeId = post.PostTypeId,
            ParentIssuerType = post.ParentIssuerType,
            SurveyId = post.SurveyId,
            ShareCount = post.ShareCount,
            Uuid = post.Uuid,
            ContentUuid = post.ContentUuid,
            ParentUuid = post.ParentUuid,
            Edited = post.Edited,
            EditDateTime = post.EditDateTime,
            Attachments = JsonConvert.DeserializeObject<List<Attachment>>(post.Attachments)
        };
        return postEntity;
    } 
    
    public static Models.Comment ToEntity(this Dto.Comment comment)
    {
        var commentEntity = new Models.Comment
        {
            Content = comment.Content,
            ContentTypeId = comment.ContentTypeId,
            RegDateTime = comment.RegDateTime,
            ChildCount = comment.ChildCount,
            Depth = comment.Depth,
            Uuid = comment.Uuid,
            ParentUuid = comment.ParentUuid,
            ContentUuid = comment.ContentUuid,
            CommentContentUuid = comment.CommentContentUuid
        };
        return commentEntity;
    }
}