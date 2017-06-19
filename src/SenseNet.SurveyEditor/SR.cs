using SenseNet.ContentRepository.i18n;

namespace SenseNet.SurveyEditor
{
    internal class SR
    {
        internal class Exceptions
        {
            internal class Survey
            {
                public static string OnlySingleResponse = "$Error_ContentRepository:Survey_OnlySingleResponse_1";
            }

            
        }

        public static string GetString(string fullResourceKey)
        {
            return SenseNetResourceManager.Current.GetString(fullResourceKey);
        }

        public static string GetString(string fullResourceKey, params object[] args)
        {
            return string.Format(GetString(fullResourceKey), args);
        }
    }
}
