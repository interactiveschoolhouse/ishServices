using IshServices.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IshServices.Data
{
    public class EventRegistrationRepository
    {
        private MySqlConnection _cn;
        public EventRegistrationRepository(MySqlConnection cn)
        {
            _cn = cn;
        }

        public void Save(EventRegistration registration)
        {
            MySqlCommand cmd = _cn.CreateCommand();

            cmd.CommandText = @"insert into Registrations(RegistrationId, RegistrationData, Created, Completed)
                Values(@RegistrationId, @RegistrationData, @Created, @Completed)";
            cmd.CommandType = System.Data.CommandType.Text;

            cmd.Parameters.AddWithValue("@RegistrationId", registration.RegistrationId);
            cmd.Parameters.AddWithValue("@RegistrationData",Newtonsoft.Json.JsonConvert.SerializeObject(registration.CustomerInfo));
            cmd.Parameters.AddWithValue("@Created", registration.Created);
            cmd.Parameters.AddWithValue("@Completed", registration.Completed);

            cmd.ExecuteNonQuery();
        }

        public EventRegistration Get(long id)
        {
            MySqlCommand cmd = _cn.CreateCommand();

            cmd.CommandText = @"select RegistrationId, RegistrationData, Created, Completed From Registrations Where RegistrationId = @RegistrationId";
            cmd.CommandType = System.Data.CommandType.Text;

            cmd.Parameters.AddWithValue("@RegistrationId", id);

            EventRegistration registration = null;

            using(MySqlDataReader r = cmd.ExecuteReader())
            {
                if (r.Read())
                {
                    registration = new EventRegistration();
                    registration.RegistrationId = r.GetInt64(0);
                    registration.CustomerInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerRegistration>(r.GetString(1));
                    registration.Created = r.GetDateTime(2);
                    registration.Completed = r.GetBoolean(3);
                }

            }

            return registration;

        }

    }
}