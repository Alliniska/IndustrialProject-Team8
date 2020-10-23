using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuizAPI.Models;

namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LobbiesController : ControllerBase
    {
        private readonly LobbyContext _context;

        public LobbiesController(LobbyContext context)
        {
            _context = context;
        }

        ////**** GET ****////
        // GET: api/Lobbies
        // READ ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lobby>>> GetLobbyItems()
        {
            //Setup the connection and command
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM lobby;", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader(); //Executes the command and returns data to the datareader

            if(data.HasRows == false) //Check if any record exists
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            //Create list of lobbies and add data from the datareader 
            List<Lobby> lobbies = new List<Lobby>();
            Lobby tmp;

            while(data.Read())
            {
                tmp = new Lobby();
                tmp.id = data.GetInt32(0);
                tmp.requestURL = data.GetString(1);
                tmp.date = data.GetDateTime(2).ToString();

                lobbies.Add(tmp);
            }

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return lobbies;
        }

        // GET: api/Lobbies/[id]
        // READ
        [HttpGet("{id}")]
        public async Task<ActionResult<Lobby>> GetLobby(int id)
        {
            //Setup the connection and command 
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM lobby WHERE id="+id+";", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader(); //Executes the command and returns data to the datareader

            if(data.HasRows == false) //Check if any record exists
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            //Create a lobby object and add data from the datareader 
            Lobby lobby = new Lobby();

            data.Read();
            lobby.id = data.GetInt32(0);
            lobby.requestURL = data.GetString(1);
            lobby.date = data.GetDateTime(2).ToString();

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return lobby;
        }

        ////**** PUT ****////
        // PUT: api/Lobbies/[id]
        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLobby(int id, Lobby lobby)
        {
            if(id != lobby.id) //Check if id in url is same as id in the body
            {
                return BadRequest();
            }

            //Setup the conenction and command 
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM lobby WHERE id=" + id + ";", cnn);

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
            cmd = new SqlCommand("UPDATE lobby " +
                                            "SET requestURL = '" + lobby.requestURL +
                                            "', date = '" + lobby.date +
                                            "' WHERE Id = " + id + ";", cnn);

            cmd.ExecuteNonQuery(); //Executes the command 
            cmd.Dispose();
            cnn.Close();

            return NoContent();
        }

        ////**** POST ****////
        // POST: api/Lobbies
        // INSERT
        [HttpPost]
        public async Task<ActionResult<Lobby>> PostLobby(Lobby lobby)
        {
            //Setup the connection
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);

            //Setup the random generator
            Random rnd = new Random();
            int id = rnd.Next(10000000, 99999999);


            //Change the command to insert the record
            SqlCommand cmd = new SqlCommand("INSERT INTO lobby(id, requestURL, date) " +
                                 "VALUES(" + id + ", '" + lobby.requestURL + "', '" + lobby.date + "');", cnn);

            cnn.Open();
            cmd.ExecuteNonQuery(); //Executes the command
            cmd.Dispose();
            cnn.Close();

            //create copy of new lobby to return
            Lobby l = new Lobby();
            l.id = id;
            l.requestURL = lobby.requestURL;
            l.date = lobby.date;
            return l;
        }

        ////**** DELETE ****////
        // DELETE: api/Lobbies/[id]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lobby>> DeleteLobby(int id)
        {
            //Setup the connection and command
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT id FROM lobby WHERE id=" + id + ";", cnn);

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
            cmd = new SqlCommand("DELETE FROM lobby WHERE id = " + id + ";", cnn);
            cmd.ExecuteNonQuery(); //Executes the command

            cmd.Dispose();
            cnn.Close();

            return Ok("Lobby deleted");
        }

        private bool LobbyExists(int id)
        {
            return _context.LobbyItems.Any(e => e.id == id);
        }
    }
}