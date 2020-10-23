using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuizAPI.Models;

namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerContext _context;

        public PlayersController(PlayerContext context)
        {
            _context = context;
        }

        ////**** GET ****////
        // GET: api/Players
        // READ ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayerItems()
        {
            //Setup the connection and command
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM player;", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader(); //Executes the command and returns data to the datareader

            if(data.HasRows == false) //Check if any record exists
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            //Create list of players and add data from the datareader 
            List<Player> players = new List<Player>();
            Player tmp;

            while(data.Read())
            {
                tmp = new Player();
                tmp.id = data.GetInt32(0);
                tmp.name = data.GetString(1);
                tmp.score = data.GetInt32(2);
                tmp.lobbyId = data.GetInt32(3);
                tmp.questionIndex = data.GetInt32(4);
                tmp.lifeline5050 = data.GetBoolean(5);
                tmp.lifelineSkip = data.GetBoolean(6);

                players.Add(tmp);
            }

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return players;
        }

        // POST: api/Players/getinfo
        // READ
        [HttpPost("getinfo")]
        public async Task<ActionResult<Player>> GetPlayer(Player curPlayer)
        {
            //Setup the connection and command
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader(); //Executes the command and returns data to the datareader

            if(data.HasRows == false) //Check if any record exists
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound(curPlayer.name + " " + curPlayer.lobbyId);
            }

            //Create a player object and add data from the datareader 
            Player player = new Player();

            data.Read();
            player.id = data.GetInt32(0);
            player.name = data.GetString(1);
            player.score = data.GetInt32(2);
            player.lobbyId = data.GetInt32(3);
            player.questionIndex = data.GetInt32(4);
            player.lifeline5050 = data.GetBoolean(5);
            player.lifelineSkip = data.GetBoolean(6);

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return player;
        }

        // GET: api/Players/inlobby/[id]
        // READ ALL FROM A LOBBY
        [HttpGet("inlobby/{lobbyId}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersInLobby(int lobbyId)
        {
            //Setup the connection and command 
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM player WHERE lobbyId=" + lobbyId + ";", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader(); //Executes the command and returns data to the datareader

            if(data.HasRows == false) //Check if any record exists
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound("Tried GetPlayersInLobby() with id " + lobbyId);
            }

            //Create list of players and add data from the datareader 
            List<Player> players = new List<Player>();
            Player tmp;

            while(data.Read())
            {
                tmp = new Player();
                tmp.id = data.GetInt32(0);
                tmp.name = data.GetString(1);
                tmp.score = data.GetInt32(2);
                tmp.lobbyId = data.GetInt32(3);
                tmp.questionIndex = data.GetInt32(4);
                tmp.lifeline5050 = data.GetBoolean(5);
                tmp.lifelineSkip = data.GetBoolean(6);

                players.Add(tmp);
            }

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return players;
        }

        // POST: api/Players/lifeline5050
        // READ LIFELINE5050
        [HttpPost("lifeline5050")]
        public async Task<ActionResult<bool>> GetPlayerLifeline5050(Player curPlayer)
        {
            //Setup the connection and command 
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT lifeline5050 FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader(); //Executes the command and returns data to the datareader

            if(data.HasRows == false) //Check if any record exists
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound("Could not find any information regarding GetPlayerLifeline5050() with name " + curPlayer.name + " and lobbyID " + curPlayer.lobbyId);
            }

            data.Read();
            bool result = data.GetBoolean(0); //Create a variable and add data from the datareader 

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return result;
        }

        // POST: api/Players/lifelineSkip
        // READ LIFELINESKIP
        [HttpPost("lifelineSkip")]
        public async Task<ActionResult<bool>> GetPlayerLifelineSkip(Player curPlayer)
        {
            //Setup the connection and command 
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT lifelineSkip FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader(); //Executes the command and returns data to the datareader

            if(data.HasRows == false) // Check if any record exists
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound("Could not find any information regarding GetPlayerLifelineSkip() with name " + curPlayer.name + "and lobbyID " + curPlayer.lobbyId);
            }

            data.Read();
            bool result = data.GetBoolean(0); //Create a variable and add data from the datareader 

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return result;
        }

        ////**** PUT ****////
        // PUT: api/Players
        // UPDATE
        [HttpPut]
        public async Task<IActionResult> PutPlayer(Player curPlayer)
        {
            //Setup the connection and command
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader(); //Executes the command and returns data to the datareader

            if(data.HasRows == false) //Check if any record exists 
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            data.Close();
            cmd.Dispose();

            //Change the command to update the record
            cmd = new SqlCommand("UPDATE player " +
                                            "SET playerName = '" + curPlayer.name +
                                            "', score = " + curPlayer.score +
                                            ", lobbyId = " + curPlayer.lobbyId +
                                            ", questionIndex = " + curPlayer.questionIndex +
                                            ", lifeline5050 = '" + curPlayer.lifeline5050 +
                                            "', lifelineSkip = '" + curPlayer.lifelineSkip +
                                            "' WHERE playerName = '" + curPlayer.name +
                                            "' AND lobbyId = " + curPlayer.lobbyId, cnn);

            cmd.ExecuteNonQuery(); //Executes the command
            cmd.Dispose();
            cnn.Close();

            return NoContent();
        }

        // PUT: api/Players/lifelines
        // UPDATE LIFELINES
        [HttpPut("lifelines")]
        public async Task<IActionResult> PutLifeline(Player curPlayer)
        {
            //Setup the connection and command 
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader(); //Executes the command and returns data to the datareader

            if(data.HasRows == false) // Check if any record exists 
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            data.Close();
            cmd.Dispose();

            //Change the command to update the record
            cmd = new SqlCommand("UPDATE player + SET lifeline5050 = '" + curPlayer.lifeline5050.ToString() +
                                            "', lifelineSkip = '" + curPlayer.lifelineSkip.ToString() +
                                            "' WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);

            cmd.ExecuteNonQuery(); //Executes the command
            cmd.Dispose();
            cnn.Close();

            return NoContent();
        }

        ////**** POST ****////
        // POST: api/Players
        // INSERT
        [HttpPost]
		public async Task<ActionResult<Player>> PostPlayer(Player player)
		{
            //Setup the connection and command 
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO player(playerName, score, lobbyId, questionIndex, lifeline5050, lifelineSkip) " +
                                            "VALUES('" + player.name + "', "  + player.score + ", " + player.lobbyId + ", " + player.questionIndex + ", '" + player.lifeline5050.ToString() + "', '" + player.lifelineSkip.ToString() + "');", cnn);

            cnn.Open();
            cmd.ExecuteNonQuery(); //Executes the command
            cmd.Dispose();

            //Change the command to select the record
            cmd = new SqlCommand("SELECT * FROM player WHERE playerName = '" + player.name + "' AND lobbyId = " + player.lobbyId + ";", cnn);

            SqlDataReader data = cmd.ExecuteReader(); //Executes the command and returns data to the datareader
            data.Read();
            int id = data.GetInt32(0); //Create a variable and add data from the datareader 

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return Ok(id);
        }

        ////**** DELETE ****////
        // DELETE: api/Players
        // DELETE
        [HttpDelete]
        public async Task<ActionResult<Player>> DeletePlayer(Player curPlayer)
        {
            //Setup the conenction and command 
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader(); //Executes the command and returns data to the datareader

            if(data.HasRows == false) //Check if any record exists
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            data.Close();
            cmd.Dispose();

            //Change the command to delete the record
            cmd = new SqlCommand("DELETE FROM player WHERE playerName = '" + curPlayer.name + "' AND lobbyId = " + curPlayer.lobbyId, cnn);
            cmd.ExecuteNonQuery(); //Executes the command

            cmd.Dispose();
            cnn.Close();

            return Ok("Player deleted");
        }

        // DELETE: api/Players/inlobby/[id]
        // DELETE ALL PLAYERS FROM A LOBBY
        [HttpDelete("inlobby/{lobbyId}")]
        public async Task<ActionResult<Player>> DeleteAllPlayersInLobby(int lobbyId)
        {
            //Setup the connection and command 
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM player WHERE lobbyId=" + lobbyId + ";", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader(); //Executes the command and returns data to the datareader

            if(data.HasRows == false) //Check if any record exists
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound("Could not find any players with lobby id " + lobbyId);
            }

            data.Close();
            cmd.Dispose();

            //Change the command to delete the record
            cmd = new SqlCommand("DELETE FROM player WHERE lobbyId = " + lobbyId + ";", cnn);
            cmd.ExecuteNonQuery(); //Executes the command 

            cmd.Dispose();
            cnn.Close();

            return Ok("All players in the lobby has been deleted");
        }

        private bool PlayerExists(int id)
        {
            return _context.PlayerItems.Any(e => e.id == id);
        }
    }
}