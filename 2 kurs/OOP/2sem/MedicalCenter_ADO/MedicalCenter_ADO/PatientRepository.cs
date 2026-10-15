using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter_ADO
{
    public class PatientRepository : IRepository<Patient>
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;

        public PatientRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        

        public bool Insert(Patient patient)
        {
            var cmd = new SqlCommand("INSERT INTO Patients (FullName, BirthDate) VALUES (@FullName, @BirthDate)", _connection, _transaction);
            cmd.Parameters.AddWithValue("@FullName", patient.FullName);
            cmd.Parameters.AddWithValue("@BirthDate", patient.BirthDate);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(Patient patient)
        {
            var cmd = new SqlCommand("UPDATE Patients SET FullName=@FullName, BirthDate=@BirthDate WHERE PatientID=@PatientID", _connection, _transaction);
            cmd.Parameters.AddWithValue("@PatientID", patient.PatientID);
            cmd.Parameters.AddWithValue("@FullName", patient.FullName);
            cmd.Parameters.AddWithValue("@BirthDate", patient.BirthDate);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            var cmd = new SqlCommand("DELETE FROM Patients WHERE PatientID=@PatientID", _connection, _transaction);
            cmd.Parameters.AddWithValue("@PatientID", id);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
