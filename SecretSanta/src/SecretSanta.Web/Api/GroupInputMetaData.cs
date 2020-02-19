using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SecretSanta.Web.Api
{
    [ModelMetadataType(typeof(GroupInputMetaData))]
    public partial class GroupInputMetaData
    {

    }

    public class GGroupInputMetaData
    {
        [Display(Name = "Title")]
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public string Title { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
