using System;
using System.Collections.Generic;

namespace CGEvents.Models
{
    public partial class QuestionnaireDetails
    {
        public int? QuestionId { get; set; }
        public byte? StarRating { get; set; }
        public int? RespondentId { get; set; }
        public int Id { get; set; }
    }
}
