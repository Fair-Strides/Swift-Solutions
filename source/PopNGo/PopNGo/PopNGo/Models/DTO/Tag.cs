using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PopNGo.Models.DTO
{
    public class Tag
    {
        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class TagExtensions
    {
        public static Models.DTO.Tag ToDTO(this Models.Tag tag)
        {
            return new Models.DTO.Tag
            {
                BackgroundColor = tag.BackgroundColor,
                TextColor = tag.TextColor
            };
        }
    }
}