using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracNghiem.Domain.Entities
{
    public enum QuestionTypeEnum
    {
        SingleChoice = 1,
        MultipleChoice
    }
    public enum SortOrderBy
    {
        Newest = 1,
        Lastest,
        DescTitle,
        AscTitle
    }
}
