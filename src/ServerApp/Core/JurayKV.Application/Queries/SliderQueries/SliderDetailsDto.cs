using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Queries.SliderQueries
{
    public class SliderDetailsDto
    {
        public Guid Id { get; set; }
        public string? Url { get; set; }
        public string? Key { get; set; }

        [Display(Name = "Second Image")]
        public string? SecondUrl { get; set; }
        public string? SecondKey { get; set; }

        public string? YoutubeVideo { get; set; }

        [Display(Name = "Is Video")]
        public bool IsVideo { get; set; }

        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        public bool Show { get; set; }

        [Display(Name = "Title")]
        public string? Title { get; set; }

        [Display(Name = "Mini Title")]
        public string? MiniTitle { get; set; }

        [Display(Name = "Text")]
        public string? Text { get; set; }


        [Display(Name = "Button Text")]
        public string? ButtonText { get; set; }

        [Display(Name = "Button Link")]
        public string? ButtonLink { get; set; }

        [Display(Name = "Disable Video Button")]
        public bool DisableVideoButton { get; set; }

        [Display(Name = "Video Link")]
        public string? VideoLink { get; set; }

        [Display(Name = "Video Button Text")]
        public string? VideoButtonText { get; set; }
    }
}
