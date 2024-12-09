﻿using System.Data;
using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;


namespace CoffeeCrazy.Repos
{
    public class ValidateDataRepo
    {
        //Gamle AssignmentJunctionRepo hvor jeg fik lavet et par databasevalideringsmetoder, som vi måske
        //kan bruge andetsteds - Gorm.
        
        private readonly string _connectionString;

        //CTOR
        public ValidateDataRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Metode til at kontrollere om assignmentSetId eksisterer i databasen.
        /// </summary>
        /// <returns> Returnere en bool, som er taget fra Sql's EXISTS, der returnerer 0 eller 1.
        /// 0 & 1 konverterer vi dermed til true eller false.
        /// </returns>
        private async Task<bool> DoesAssignmentSetIdExistAsync(int assignmentSetId)
        {
            string query = @"
                        SELECT CASE WHEN EXISTS (
                            SELECT AssignmentSetId
                            FROM AssignmentSets
                            WHERE AssignmentSetId = @AssignmentSetId)
                            THEN 1 ELSE 0 END";

                                                    
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@AssignmentSetId", SqlDbType.Int).Value = assignmentSetId;

                    var result = await command.ExecuteScalarAsync();

                    return Convert.ToInt32(result) == 1;
                }

            }
        }

        /// <summary>
        /// Metode til at kontrollere om databasen indeholder ens AssignmentId-Query
        /// </summary>
        /// <returns> returnerer en bool-lignende værdi, igennem Sql Exist Query. 
        /// Exist returnere 0 eller 1, hvilket svarer til true eller false, men vi skal have konverteret det først.        /// 
        /// </returns>
        //Metode til at kontrollere om databasen indeholde ens AssignmentId-query. 
        private async Task<bool> DoesAssignmentIdExistAsync(int assignmentId)
        {
            string query = @"
                            SELECT CASE WHEN EXISTS (
                            SELECT AssignmentId
                            FROM Tasks
                            WHERE AssignmentId = @AssignmentId)
                            THEN 1 ELSE 0 END";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@AssignmentId", SqlDbType.Int).Value = assignmentId;
                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result) == 1;
                }

            }
        }

        /// <summary>
        /// En metode til at validere om vores billede i databasen indeholder værdier.
        /// Evt. give den et andet navn - alla ControlForImageValue eller sådan.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> returnerer en byte hvis der er noget i den gældende kolonne. Ellers sætter den kolonnen til at indeholde null, hvilekt den godt må </returns>
        //public static byte[]? GetImageValue(object value)
        //{
        //    if (value != DBNull.Value)  //validere at vores værdi ikke er null så der kunne komme en 
        //    {
        //        return (byte[])value;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}


    }
}
