using NebulaGraphDemo.Models;
using Newtonsoft.Json;

namespace NebulaGraphDemo.Extensions;

public static class Mapper
{
    public static Dto.Post ToDto(this PostEntity postEntity)
    {
        return new Dto.Post
        {
            IssuerId = postEntity.IssuerId,
            IssuerType = postEntity.IssuerType,
            RegDate = postEntity.RegDate,
            RegTime = postEntity.RegTime,
            IsShareable = postEntity.IsShareable,
            IsCommentable = postEntity.IsCommentable,
            IsPublic = postEntity.IsPublic,
            PostType = postEntity.PostType,
            RegUser = postEntity.RegUser,
            UniqueId = postEntity.UniqueId,
            RegDateTime = postEntity.RegDateTime,
            LikeCount = postEntity.LikeCount,
            CommentCount = postEntity.CommentCount,
            ViewCount = postEntity.ViewCount,
            Content = postEntity.Content,
            Attachments = JsonConvert.SerializeObject(postEntity.Attachments),
            IssuerPostId = postEntity.IssuerPostId,
            IsSurvey = postEntity.IsSurvey,
            PostTypeId = postEntity.PostTypeId,
            ParentIssuerType = postEntity.ParentIssuerType,
            SurveyId = postEntity.SurveyId,
            ShareCount = postEntity.ShareCount,
            uuid = postEntity.Uuid,
            ContentUuid = postEntity.ContentUuid,
            ParentUuid = postEntity.ParentUuid,
            Edited = postEntity.Edited,
            EditDateTime = postEntity.EditDateTime,
        };
    }

    public static PostEntity ToEntity(this Dto.Post post)
    {
        var postEntity = new PostEntity
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
            Uuid = post.uuid,
            ContentUuid = post.ContentUuid,
            ParentUuid = post.ParentUuid,
            Edited = post.Edited,
            EditDateTime = post.EditDateTime,
            Attachments = JsonConvert.DeserializeObject<List<Attachment>>(post.Attachments)
        };
        return postEntity;
    }
    
    public static Comment ToEntity(this Dto.Comment comment)
    {
        var commentEntity = new Comment
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