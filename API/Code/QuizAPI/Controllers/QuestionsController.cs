﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuizAPI.Models;

namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly QuestionContext _context;

        public QuestionsController(QuestionContext context)
        {
            _context = context;
        }

        ////**** GET ****////
        // GET: api/Questions
        // READ ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestionItems()
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM question;", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            //create question list
            List<Question> questions = new List<Question>();
            Question tmp;

            while(data.Read())
            {
                tmp = new Question();
                tmp.id = data.GetInt32(0);
                tmp.category = data.GetString(1);
                tmp.type = data.GetString(2);
                tmp.difficulty = data.GetString(3);
                tmp.question = data.GetString(4);
                tmp.correctAnswer = data.GetString(5);
                tmp.incorrectAnswer1 = data.GetString(6);
                tmp.incorrectAnswer2 = data.GetString(7);
                tmp.incorrectAnswer3 = data.GetString(8);
                tmp.questionIndex = data.GetInt32(9);
                tmp.lobbyId = data.GetInt32(10);

                questions.Add(tmp);
            }

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return questions;
        }

        // GET: api/Questions/getinfo
        // READ
        [HttpGet("getinfo")]
        public async Task<ActionResult<Question>> GetQuestionSingle(Question q)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM question WHERE questionIndex = " + q.questionIndex + " AND lobbyId = " + q.lobbyId + ";", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            //create question object
            Question question = new Question();

            data.Read();
            question.id = data.GetInt32(0);
            question.category = data.GetString(1);
            question.type = data.GetString(2);
            question.difficulty = data.GetString(3);
            question.question = data.GetString(4);
            question.correctAnswer = data.GetString(5);
            question.incorrectAnswer1 = data.GetString(6);
            question.incorrectAnswer2 = data.GetString(7);
            question.incorrectAnswer3 = data.GetString(8);
            question.questionIndex = data.GetInt32(9);
            question.lobbyId = data.GetInt32(10);

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return question;
        }

        // GET: api/Questions/inlobby/[id]
        // READ ALL FROM A LOBBY
        [HttpGet("inlobby/{lobbyId}")]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestionsInLobby(int lobbyId)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM question WHERE lobbyId = " + lobbyId + ";", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound("Tried GetQuestionsInLobby() with id " + lobbyId);
            }

            //create questions list
            List<Question> questions = new List<Question>();
            Question tmp;

            while(data.Read())
            {
                tmp = new Question();
                tmp.id = data.GetInt32(0);
                tmp.category = data.GetString(1);
                tmp.type = data.GetString(2);
                tmp.difficulty = data.GetString(3);
                tmp.question = data.GetString(4);
                tmp.correctAnswer = data.GetString(5);
                tmp.incorrectAnswer1 = data.GetString(6);
                tmp.incorrectAnswer2 = data.GetString(7);
                tmp.incorrectAnswer3 = data.GetString(8);
                tmp.questionIndex = data.GetInt32(9);
                tmp.lobbyId = data.GetInt32(10);

                questions.Add(tmp);
            }

            data.Close();
            cmd.Dispose();
            cnn.Close();

            return questions;
        }



        ////**** PUT ****////
        // PUT: api/Questions
        // UPDATE
        [HttpPut]
        public async Task<IActionResult> PutQuestion(Question question)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM question WHERE questionIndex = " + question.questionIndex + " AND lobbyId = " + question.lobbyId + ";", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            data.Close();
            cmd.Dispose();

            connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            cmd = new SqlCommand("UPDATE question " +
                                            "SET category = '" + question.category +
                                            "', type = '" + question.type +
                                            "', difficulty = '" + question.difficulty +
                                            "', question = '" + question.question +
                                            "', correctAnswer = '" + question.correctAnswer +
                                            "', incorrectAnswer1 = '" + question.incorrectAnswer1 +
                                            "', incorrectAnswer2 = '" + question.incorrectAnswer2 +
                                            "', incorrectAnswer3 = '" + question.incorrectAnswer3 +
                                            "', questionIndex = " + question.questionIndex +
                                            ", lobbyId = " + question.lobbyId +
                                            " WHERE questionIndex = " + question.questionIndex + " AND lobbyId = " + question.lobbyId + ";", cnn); ;

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();

            return NoContent();
        }



        ////**** POST ****////
        // POST: api/Questions
        // INSERT
        [HttpPost]
        public async Task<ActionResult<string>> PostQuestion(Question question)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO question(category, type, difficulty, question, correctAnswer, incorrectAnswer1, incorrectAnswer2, incorrectAnswer3, questionIndex, lobbyId) " +
                                 "VALUES('" + question.category + "', '" +  question.type + "', '" +  question.difficulty + "', '" + question.question + "', '" + question.correctAnswer + "', '" +  question.incorrectAnswer1 + "', '" + question.incorrectAnswer2 + "', '" + question.incorrectAnswer3 + "', " + question.questionIndex + ", " + question.lobbyId + ");", cnn);

            cnn.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                return e.ToString() + " " + e.Message;
            }
            cmd.Dispose();
            cnn.Close();

            return Ok();
        }

        // POST: api/Questions/list
        // INSERT A LIST
        [HttpPost("list")]
        public async Task<ActionResult<string>> PostListOfQuestion(Question[] questions)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd;

            cnn.Open();
            for(int i = 0; i < questions.Length; i++)
			{
                cmd = new SqlCommand("INSERT INTO question(category, type, difficulty, question, correctAnswer, incorrectAnswer1, incorrectAnswer2, incorrectAnswer3, questionIndex, lobbyId) " +
                                 "VALUES('" + questions[i].category + "', '" + questions[i].type + "', '" + questions[i].difficulty + "', '" + questions[i].question + "', '" + questions[i].correctAnswer + "', '" + questions[i].incorrectAnswer1 + "', '" + questions[i].incorrectAnswer2 + "', '" + questions[i].incorrectAnswer3 + "', " + questions[i].questionIndex + ", " + questions[i].lobbyId + ");", cnn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            cnn.Close();

            return Ok();
        }



        ////**** DELETE ****////
        // DELETE: api/Questions
        [HttpDelete]
        public async Task<ActionResult<Question>> DeleteQuestion(Question question)
        {
            string connetionString = "Data Source=riddlers.database.windows.net;Initial Catalog=quizgame;User ID=team8;Password=b7zYDzhJ;";
            SqlConnection cnn = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM question WHERE questionIndex = " + question.questionIndex + " AND lobbyId = " + question.lobbyId + ";", cnn);

            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();

            if(data.HasRows == false)
            {
                data.Close();
                cmd.Dispose();
                cnn.Close();
                return NotFound();
            }

            data.Close();
            cmd.Dispose();

            cmd = new SqlCommand("DELETE FROM question WHERE questionIndex = " + question.questionIndex + " AND lobbyId = " + question.lobbyId + ";", cnn);
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            cnn.Close();

            return Ok("Question deleted");
        }

        private bool QuestionExists(int id)
        {
            return _context.QuestionItems.Any(e => e.id == id);
        }
    }
}