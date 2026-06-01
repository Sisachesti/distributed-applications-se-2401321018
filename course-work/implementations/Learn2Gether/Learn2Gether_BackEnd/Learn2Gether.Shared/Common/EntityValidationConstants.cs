using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Shared.Common
{
    public static class EntityValidationConstants
    {
        public static class ModuleConstraints
        {
            public const int TitleMaxLength = 50;
        }
        public static class CourseConstraints
        {
            public const int TitleMaxLength = 50;
            public const int MaxRating = 5;
            public const int MaxDurationInMinutes = 7200;
            public const int MaxDescription = 1000;
            public const int ImageUrlMaxChars = 2048;
        }

        public static class LectureConstraints
        {
            public const int TitleMaxLength = 50;
            public const int VideoUrlMaxLength = 2048;
        }

        public static class AnswerConstraints
        {
            public const int TitleMaxLength = 50;
            public const int ContentMaxLength = 1000;
        }
    }
}
