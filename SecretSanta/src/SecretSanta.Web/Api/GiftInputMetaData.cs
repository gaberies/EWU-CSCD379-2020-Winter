using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SecretSanta.Web.Api
{
    [ModelMetadataType(typeof(GiftInputMetaData))]
    public partial class GiftInput
    {

    }

    public class GiftInputMetaData
    {
        [Display(Name = "User Id")]
        public int UserId { get; set; }
    }
}