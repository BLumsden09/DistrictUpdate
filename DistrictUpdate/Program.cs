using System.Data;
using System.Data.SqlClient;

namespace DistrictUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Connect to the SQL Database*/
            using (
                var conn = new SqlConnection("Server=0;Database=0;User ID=0;Password= 0;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;")
                )
            {
                conn.Open();

                /*Query the District table to place result in a DataTable*/
                string districtQuery = "SELECT leaID, NCESID_District FROM District";
                SqlCommand dcmd = new SqlCommand(districtQuery, conn);
                SqlDataAdapter district_da = new SqlDataAdapter(dcmd);
                DataTable district = new DataTable();
                district_da.Fill(district);

                /*Query the Staff table to place result in a DataTable*/
                string staffQuery = "SELECT leaID, staffID FROM Staff";
                SqlCommand stcmd = new SqlCommand(staffQuery, conn);
                SqlDataAdapter staff_da = new SqlDataAdapter(stcmd);
                DataTable staff = new DataTable();
                staff_da.Fill(staff);

                /*Query the School table to place result in a DataTable*/
                string schoolQuery = "SELECT leaID, schoolID, NCESID_District FROM School";
                SqlCommand scmd = new SqlCommand(schoolQuery, conn);
                SqlDataAdapter school_da = new SqlDataAdapter(scmd);
                DataTable school = new DataTable();
                school_da.Fill(school);

                /*Take the leaID column in each DataTable and pad the row values to the left.*/
                for (int i = 0; i < district.Rows.Count; i++)
                {
                    var leaID = district.Rows[i]["leaID"].ToString().PadLeft(5, '0');
                    district.Rows[i]["leaID"] = leaID;
                }

                for (int j = 0; j < staff.Rows.Count; j++)
                {
                    var leaID = staff.Rows[j]["leaID"].ToString().PadLeft(5, '0');
                    staff.Rows[j]["leaID"] = leaID;
                }

                for (int k = 0; k < school.Rows.Count; k++)
                {
                    var leaID = school.Rows[k]["leaID"].ToString().PadLeft(5, '0');
                    school.Rows[k]["leaID"] = leaID;
                }

                /*Run query to update the leaID value in the District, School, and Staff database tables.*/
                for (int l = 0; l < district.Rows.Count; l++)
                {
                    SqlCommand command = new SqlCommand("UPDATE District SET leaID = @leaID WHERE NCESID_District = @ncesid", conn);
                    command.Parameters.AddWithValue("@leaID", district.Rows[l][0]);
                    command.Parameters.AddWithValue("@ncesid", district.Rows[l][1]);
                    command.ExecuteNonQuery();

                }

                for (int m = 0; m < staff.Rows.Count; m++)
                {
                    SqlCommand command = new SqlCommand("UPDATE Staff SET leaID = @leaID WHERE staffID = @staffID", conn);
                    command.Parameters.AddWithValue("@leaID", staff.Rows[m][0]);
                    command.Parameters.AddWithValue("@staffID", staff.Rows[m][1]);
                    command.ExecuteNonQuery();

                }

                for (int n = 0; n < school.Rows.Count; n++)
                {
                    SqlCommand command = new SqlCommand("UPDATE School SET leaID = @leaID WHERE schoolID = @schoolID AND NCESID_District = @ncesid", conn);
                    command.Parameters.AddWithValue("@leaID", school.Rows[n][0]);
                    command.Parameters.AddWithValue("@schoolID", school.Rows[n][1]);
                    command.Parameters.AddWithValue("@ncesid", school.Rows[n][2]);
                    command.ExecuteNonQuery();

                }

                /*Close the database connection and dispose of each DataTable*/
                conn.Close();
                district_da.Dispose();
                staff_da.Dispose();
                school_da.Dispose();

            }
        }
    }
}
