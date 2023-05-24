using System;
using System.Collections.Generic;
using System.Linq;
using AdminPanel.Entities;

namespace AdminPanel.Converters
{
    public static partial class UserMapper
    {
        public static UserPlainObject AdaptToPlainObject(this User p1)
        {
            return p1 == null ? null : new UserPlainObject()
            {
                Id = p1.Id,
                Email = p1.Email,
                Fullname = p1.Fullname,
                PhoneNumber = p1.PhoneNumber,
                Gender = p1.Gender,
                BirthDate = p1.BirthDate,
                IsBanned = p1.IsBanned,
                Restaurant = p1.Restaurant,
                Roles = string.Join(Environment.NewLine, p1.Roles),
                AllUserClaims = string.Join(Environment.NewLine, p1.AllUserClaims.Select<KeyValuePair<string, string>, string>(funcMain1))
            };
        }
        
        private static string funcMain1(KeyValuePair<string, string> claim)
        {
            return string.Format("{0} = {1}", claim.Key, claim.Value);
        }
    }
}