﻿using IshServices.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace IshServices.Data
{
    public class ClassRegistrationRepository
    {
        private UnitOfWork _unitOfWork;
        public ClassRegistrationRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Save(ClassRegistration registration)
        {
            MySqlCommand cmd = _unitOfWork.CreateCommand(@"insert into Registrations(RegistrationId, RegistrationData, Created, Completed)
                Values(@RegistrationId, @RegistrationData, @Created, @Completed)",
                System.Data.CommandType.Text);


            cmd.Parameters.AddWithValue("@RegistrationId", registration.RegistrationId);
            cmd.Parameters.AddWithValue("@RegistrationData",Newtonsoft.Json.JsonConvert.SerializeObject(registration));
            cmd.Parameters.AddWithValue("@Created", registration.Created);
            cmd.Parameters.AddWithValue("@Completed", registration.Completed);

            cmd.ExecuteNonQuery();
        }

        public ClassRegistration Get(long id)
        {
            MySqlCommand cmd = _unitOfWork.CreateCommand(@"select RegistrationId, RegistrationData, Created, Completed From Registrations Where RegistrationId = @RegistrationId",
                System.Data.CommandType.Text);

            cmd.Parameters.AddWithValue("@RegistrationId", id);

            ClassRegistration registration = null;

            var contractResolver = new PrivateMemberContractResolver();
            var deserializeSettings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            };

            using (MySqlDataReader r = cmd.ExecuteReader())
            {
                if (r.Read())
                {
                    registration = JsonConvert.DeserializeObject<ClassRegistration>(r.GetString(1), deserializeSettings);
                }
            }

            return registration;

        }

    }
}