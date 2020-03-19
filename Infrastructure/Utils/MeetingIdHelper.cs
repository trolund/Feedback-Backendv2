using System;
using HashidsNet;

namespace Feedback.Utils
{
    public class MeetingIdHelper
    {
        private static Hashids hashids = new Hashids(Environment.GetEnvironmentVariable("MEETINGIDSALT"));
        public static string GenerateShortId(int id)
        {
            return hashids.Encode(id);
        }

        public static int GetId(string shortId)
        {
            return hashids.Decode(shortId)[0];
        }
    }
}
