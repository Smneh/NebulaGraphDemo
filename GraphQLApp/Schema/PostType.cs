using HotChocolate.Types;

namespace GraphQL.Schema;
using Post = NebulaGraphDemo.Dto.Post;

public class PostType : ObjectType<Post>
{
    protected override void Configure(IObjectTypeDescriptor<Post> descriptor)
    {
        descriptor.Field(p => p.Content).Type<StringType>();
        descriptor.Field(p => p.IssuerId).Type<StringType>();
        descriptor.Field(p => p.IssuerId).Type<IntType>();
        descriptor.Field(p => p.IssuerType).Type<IntType>();
        descriptor.Field(p => p.RegDate).Type<LongType>();
        descriptor.Field(p => p.RegTime).Type<LongType>();
        descriptor.Field(p => p.IsShareable).Type<BooleanType>();
        descriptor.Field(p => p.IsCommentable).Type<BooleanType>();
        descriptor.Field(p => p.IsPublic).Type<BooleanType>();
        descriptor.Field(p => p.PostType).Type<IntType>();
        descriptor.Field(p => p.RegUser).Type<StringType>();
        descriptor.Field(p => p.UniqueId).Type<StringType>();
        descriptor.Field(p => p.RegDateTime).Type<DateType>();
        descriptor.Field(p => p.LikeCount).Type<IntType>();
        descriptor.Field(p => p.CommentCount).Type<IntType>();
        descriptor.Field(p => p.ViewCount).Type<IntType>();
        descriptor.Field(p => p.Content).Type<StringType>();
        descriptor.Field(p => p.IssuerPostId).Type<IntType>();
        descriptor.Field(p => p.IsSurvey).Type<BooleanType>();
        descriptor.Field(p => p.PostTypeId).Type<LongType>();
        descriptor.Field(p => p.ParentIssuerType).Type<StringType>();
        descriptor.Field(p => p.SurveyId).Type<IntType>();
        descriptor.Field(p => p.ShareCount).Type<IntType>();
        descriptor.Field(p => p.uuid).Type<StringType>();
        descriptor.Field(p => p.ContentUuid).Type<StringType>();
        descriptor.Field(p => p.ParentUuid).Type<StringType>();
        descriptor.Field(p => p.Edited).Type<BooleanType>();
        descriptor.Field(p => p.EditDateTime).Type<DateType>();
        descriptor.Field(p => p.Attachments).Type<StringType>();
    }
}