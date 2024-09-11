using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Queries.SliderQueries
{
    public class SliderListDto
    {
        public Guid Id { get; set; }
        public string? Url { get; set; } 

        [Display(Name = "Second Image")]
        public string? SecondUrl { get; set; } 

        public string? YoutubeVideo { get; set; }

        [Display(Name = "Is Video")]
        public bool IsVideo { get; set; }

        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        public bool Show { get; set; }

        [Display(Name = "Title")]
        public string? Title { get; set; }

        
    }
}
