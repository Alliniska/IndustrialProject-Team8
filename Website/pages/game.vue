<template>
  <div class="container">
    <div class="tile is-ancestor">
      <div class="tile is-parent is-8">
        <article class="tile is-child box border"><!-- QUESTION TILE -->

            <p class="title" id="questionNumber">Question 000</p>
              <p class="subtitle" id="questionTopic">Difficulty: --- <br>Topic: --- <br/></p>
            <!--TIMER CODE TAKEN FROM https://medium.com/js-dojo/how-to-create-an-animated-countdown-timer-with-vue-89738903823f-->
              <div class="base-timer" >
                <span id="timerLabel">{{ formattedTimeLeft }}</span>
                <svg class="base-timer__svg" viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg">
                  <g class="base-timer__circle">
                    <circle class="base-timer__path-elapsed" cx="50" cy="50" r="45"></circle>
                    <path
                      :stroke-dasharray="circleDasharray"
                      class="base-timer__path-remaining"
                      :class="remainingPathColor"
                      d="
                        M 50, 50
                        m -45, 0
                        a 45,45 0 1,0 90,0
                        a 45,45 0 1,0 -90,0
                      "
                    ></path>
                  </g>
                </svg>
              </div>
          <div class="content">
            <p class="is-size-3-tablet" id="questionBox"></p>
          </div>
        </article>
      </div>
      <div class="tile is-parent is-vertical"> <!-- SCORE AND LIFELINE TILES -->
        <article class="tile is-child box border">
          <p class="title is-centered">Lifelines</p>
          <p class="subtitle is-centered">Need some help? This is the place!</p>
          <div class="buttons is-centered">
            <b-button @click="fiftyfiftyLifeline()" class="lifeline is-yellow" id="fiftyfifty">50/50</b-button>
            <b-button @click="skipLifeline()" class="lifeline is-yellow" id="skip">Skip Question</b-button>
          </div>
        </article>
        <article class="tile is-child box border">
          <p class="title is-centered" id="playerPosition">---</p>
          <p class="subtitle is-centered" id="playerScore">Score: ---</p>
          <p class="subtitle is-centered" id="streak">---</p>
        </article>
      </div>
    </div>
    <div class="tile is-ancestor">
      <!-- audio will be played when option is selcted -->
      <audio id="correct" src="~/assets/CorrectAnswer.mp3"></audio>
      <audio id="incorrect" src="~/assets/IncorrectAnswer.mp3"></audio>
      <div class="tile is-parent is-vertical buttons">
        <b-button @click="checkAnswer('ansOne')" class="tile is-child border is-white answerButton" id="ansOneA" opacity="1">
          <p class="is-size-4 answerLabel" id="ansOne">----</p>
        </b-button>
        <b-button @click="checkAnswer('ansTwo')" class="tile is-child border is-white answerButton" id="ansTwoA" opacity="1">
          <p class="is-size-4 answerLabel" id="ansTwo">----</p>
        </b-button>
      </div>
      <div class="tile is-parent is-vertical buttons">
        <b-button @click="checkAnswer('ansThree')" class="tile is-child border is-white answerButton" id="ansThreeA">
          <p class="is-size-4 answerLabel" id="ansThree">----</p>
        </b-button>
        <b-button @click="checkAnswer('ansFour')" class="tile is-child border is-white answerButton" id="ansFourA">
          <p class="is-size-4 answerLabel" id="ansFour">----</p>
        </b-button>
      </div>
      <div class="tile is-parent is-4"> <!-- LEADERBOARD TILE -->
        <article class="tile is-child box border">
          <div class="message">
            <div class="message-header">
              <p>Leaderboard</p>
            </div>
            <div class="message-body">
              <b-table
                :data="tableData"
                :columns="tableColumns"
                :perPage="5"
                paginated
                default-sort-direction="desc"
                default-sort="score"
                :paginationSimple="false"
              ></b-table>
            </div>
          </div>
        </article>
      </div>
    </div>
    <!--<button @click="getQsTokens">Refresh Data</button>
    <div>
      <h1>Lobby info</h1>
      <p v-if="$fetchState.pending">Fetching lobby details ...</p>
      <p v-else-if="$fetchState.error">
        Error while fetching lobby details: {{ $fetchState.error.message }}
      </p>
      <ul v-else>
        <p>
          {{ this.lobbyInfo }}
        </p>
      </ul>
    </div>-->

  </div> <!-- closing container tag-->
</template>

<script>

//Setting thresholds for timer countdown animation
const FULL_DASH_ARRAY = 283;
const WARNING_THRESHOLD = 10;
const ALERT_THRESHOLD = 5;

//Setting colours for the timer countdown animation
const COLOR_CODES = {
  info: {
    color: "green"
  },
  warning: {
    color: "orange",
    threshold: WARNING_THRESHOLD
  },
  alert: {
    color: "red",
    threshold: ALERT_THRESHOLD
  }
};

//Setting timer duration
const TIME_LIMIT = 30;
import router from '../router'
export default {
  name: 'game',
  metaInfo: {
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' }
    ]
  },
  data() {
    return {
        tableData: [],
        currQuestion: 0,
        lifeline5050: true,
        lifelineSkip: true,
        nickname: "",
        streak: 0,
        timePassed: 0,
        timerInterval: null,

        score: 0,
        tableColumns: [
            {
                field: 'name',
                label: 'Nickname',
                width: '40',
            },
            {
                field: 'score',
                label: 'Score',
                numeric: true,
                sortable: true,
                centered: true
            }
        ],
      lobbyInfo: [],
      allQuestions: [],
    }
  },
  created() {
    let playerInfo = this.$route.params.playerInfo;
    if (playerInfo == undefined) {
      this.$buefy.dialog.confirm({
        message: "Error finding player data - redirecting to homepage...",
        onConfirm: () => window.location.href = "/",
        onCancel: () => window.location.href = "/"
      });
    }
    let tempPlayerInfo = playerInfo.split(";");
    this.nickname = tempPlayerInfo[0].split("nickname=")[1];
    let lobbyId = tempPlayerInfo[1].split("lobbyId=")[1];
    document.getElementById("userNickname").innerHTML = this.nickname;
    document.getElementById("lobbyCode").innerHTML = lobbyId;
    let id = parseInt(lobbyId);
    this.startGame(id);
  },
  methods: {

    /*
     *  Start Game function
     *  Starts the whole game process off. Runs once, takes no parameters.
     */
    async startGame(lobbyId) {
      if (await this.getLobbyInfo(lobbyId)) {
        this.getQs();
        this.updatePlayerScoreAndPos(0);
      }
    },

    async getLobbyInfo(id) {
      this.lobbyInfo = await fetch(`/quizApi/Lobbies/${id}`).then((res) => res.json());
      return true;
    },

    /*
     *  Check Answer function
     *  Called when the player clicks any of the four answer buttons, it checks if the button pressed contained the correct answer to the
     *  question, returns the appropriate message, adjusts their score and gets the next question.
     */
    async checkAnswer(buttonId) {

        if (document.getElementById(buttonId).innerHTML == this.allQuestions[this.currQuestion].correct_answer) {
          document.getElementById(buttonId).style.backgroundColor = "green";
          document.getElementById(buttonId+"A").style.backgroundColor = "green";
          // audio for correct answer is played
          var ca = document.getElementById("correct");
          ca.play();
          this.streak+=1;
          this.updatePlayerScoreAndPos(((30-(this.timePassed))*10)*(this.streak));
        }
        else {
          document.getElementById(buttonId).style.backgroundColor = "red";
          document.getElementById(buttonId+"A").style.backgroundColor = "red";
          // audio for incorrect answer is played
          var ia = document.getElementById("incorrect")
          ia.play();
          this.streak=0;
          this.updatePlayerScoreAndPos(-500);
          if (document.getElementById("ansOne").innerHTML == this.allQuestions[this.currQuestion].correct_answer) {
            document.getElementById("ansOne").style.backgroundColor = "green";
            document.getElementById("ansOneA").style.backgroundColor = "green";
          }
          else if (document.getElementById("ansTwo").innerHTML == this.allQuestions[this.currQuestion].correct_answer) {
            document.getElementById("ansTwo").style.backgroundColor = "green";
            document.getElementById("ansTwoA").style.backgroundColor = "green";
          }
          else if (document.getElementById("ansThree").innerHTML == this.allQuestions[this.currQuestion].correct_answer) {
            document.getElementById("ansThree").style.backgroundColor = "green";
            document.getElementById("ansThreeA").style.backgroundColor = "green";
          }
          else if (document.getElementById("ansFour").innerHTML == this.allQuestions[this.currQuestion].correct_answer) {
            document.getElementById("ansFour").style.backgroundColor = "green";
            document.getElementById("ansFourA").style.backgroundColor = "green";
          }
        }
      this.disableAnswerButtons();
      setTimeout(() => {
        this.resetAnswerButtons();
        this.getNextQuestion();
      }, 2000);
    },

    disableAnswerButtons() {
      document.getElementById("ansOneA").disabled = true;
      document.getElementById("ansTwoA").disabled = true;
      document.getElementById("ansThreeA").disabled = true;
      document.getElementById("ansFourA").disabled = true;
    },

    /*
     *  Function to reset all answer button colours and re-enable them all
     */
    resetAnswerButtons() {
      document.getElementById("ansOne").style.backgroundColor = "white";
        document.getElementById("ansOneA").style.backgroundColor = "white";
        document.getElementById("ansTwo").style.backgroundColor = "white";
        document.getElementById("ansTwoA").style.backgroundColor = "white";
        document.getElementById("ansThree").style.backgroundColor = "white";
        document.getElementById("ansThreeA").style.backgroundColor = "white";
        document.getElementById("ansFour").style.backgroundColor = "white";
        document.getElementById("ansFourA").style.backgroundColor = "white";
        document.getElementById("ansOneA").disabled = false;
        document.getElementById("ansTwoA").disabled = false;
        document.getElementById("ansThreeA").disabled = false;
        document.getElementById("ansFourA").disabled = false;
    },

    /*
     *  Function to remove two incorrect answers for 50/50 lifeline
     */
    fiftyfiftyLifeline() {

      this.lifeline5050 = false;
      document.getElementById("fiftyfifty").disabled = true; //disable lifeline after 1 use
      let answer;

      if (document.getElementById("ansOne").innerHTML == this.allQuestions[this.currQuestion].correct_answer) {
        answer = 1;
      }
      else if (document.getElementById("ansTwo").innerHTML == this.allQuestions[this.currQuestion].correct_answer) {
        answer = 2;
      }
      else if (document.getElementById("ansThree").innerHTML == this.allQuestions[this.currQuestion].correct_answer) {
        answer = 3;
      }
      else if (document.getElementById("ansFour").innerHTML == this.allQuestions[this.currQuestion].correct_answer) {
        answer = 4;
      }


      let random = this.getRandomNum(1,4);
      while (random==answer) {
        random = this.getRandomNum(1,4);
      }
      let random2 = this.getRandomNum(1,4);
      while (random2==answer || random2==random) {
        random2 = this.getRandomNum(1,4);
      }


      if (random2 == 1 || random == 1) {
        document.getElementById("ansOne").style.backgroundColor = "grey";
        document.getElementById("ansOneA").style.backgroundColor = "grey";
        document.getElementById("ansOneA").disabled = true;
      }

      if (random2 == 2 || random == 2) {
        document.getElementById("ansTwo").style.backgroundColor = "grey";
        document.getElementById("ansTwoA").style.backgroundColor = "grey";
        document.getElementById("ansTwoA").disabled = true;
      }

      if (random2 == 3 || random == 3) {
        document.getElementById("ansThree").style.backgroundColor = "grey";
        document.getElementById("ansThreeA").style.backgroundColor = "grey";
        document.getElementById("ansThreeA").disabled = true;
      }

      if (random2 == 4 || random == 4) {
        document.getElementById("ansFour").style.backgroundColor = "grey";
        document.getElementById("ansFourA").style.backgroundColor = "grey";
        document.getElementById("ansFourA").disabled = true;
      }

      this.updatePlayerTable();
    },

    /*
     *  Function to skip a question without losing points or their streak for skip lifeline
     */
    skipLifeline() {
      this.lifelineSkip = false;
      document.getElementById("skip").disabled = true; //disable lifeline after 1 use
      this.updatePlayerTable();
      this.getNextQuestion();
    },

    /*
     *  Update Lobby Table function
     *  Updates the database with the questions generated in the front end so players can pull that data when they want to play
     */
    async updateLobbyTable() {
      let tempLobbyInfo = {
        id: this.lobbyInfo.id,
        date: new Date().toISOString(),
      };
      const response = await fetch(`/quizApi/Lobbies/${this.lobbyInfo.id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(this.lobbyInfo)
      }).then((res) => res.json());
    },

    /*
     *  Update Player Table function
     *  Updates the database with the Player's info, such as their score, what question they're up to etc.
     *  Currently, it sends everything back to the database, rather than just what needs updating.
     */
    async updatePlayerTable(){
      let playerInfo = {
        name: this.nickname,
        Score: this.score,
        lobbyId: this.lobbyInfo.id,
        questionIndex: this.currQuestion+1,
        Lifeline5050: this.lifeline5050,
        LifelineSkip: this.lifelineSkip
      };
      const response = await fetch('/quizApi/Players', {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(playerInfo)
      });
    },

    /*
     *  Update Player Score and Position function
     *
     *  This function has two purposes - adjusting the player's score and updating the score, position and leaderboard elements.
     *  If it is run at the start of the game, it will query the database for the current player's score and save that as their existing score.
     *  otherwise, it will adjust their score locally and then update the database with the change and pull the latest changes from the database
     *  It also updates the position shown on screen so the player knows where they place on the leaderboard
     *
     */
    async updatePlayerScoreAndPos(adjustment) {
      if (adjustment == 0) {
        this.tableData = await fetch(`/quizApi/Players/inlobby/${this.lobbyInfo.id}`).then((res) => res.json());
        this.score = this.findCurrentPlayer(false, this.tableData);

      }
      else {
        this.score += adjustment;
        let playerPos = 0;
        let tempTable = await fetch(`/quizApi/Players/inlobby/${this.lobbyInfo.id}`).then((res) => res.json()); //  fetch the latest version of the leaderboard and save it locally in a variable
        playerPos = this.findCurrentPlayer(true, tempTable);                                                    //  get the position of the current player in the leaderboard
        tempTable[playerPos].score = this.score;                                                                //  update the player's score in the local copy of the database
        this.tableData = tempTable;                                                                             //  update the table shown on screen
        this.updatePlayerTable();                                                                               //  update the database with the new score ONLY
      }
      document.getElementById("playerScore").innerHTML = `Score: ${this.score}`;                                //  update the player's score on screen
      document.getElementById("streak").innerHTML = `Streak: ${this.streak}`;
      this.tableData.sort((a,b) => b.score - a.score);                                                          //  sort the table
      let playerPos = this.findCurrentPlayer(true, this.tableData) + 1;                                         //  find the player's position on the leaderboard
      let positionElem = document.getElementById("playerPosition");
      switch (playerPos) {
        case 1:
          positionElem.innerHTML = "1st";
          break;
        case 2:
          positionElem.innerHTML = "2nd";
          break;
        case 3:
          positionElem.innerHTML = "3rd";
          break;
        default:
          positionElem.innerHTML = `${playerPos}th`;
          break;
      }
    },

    /*
     *  Find Current Player in the Table function
     *  This function finds the player's data in the latest version of the data pulled from the database.
     *  It can be used to get the player's score or their position in the leaderboard
     */
    findCurrentPlayer(position, tempTable) {
      for (let playerPos=0; playerPos < tempTable.length; playerPos++) {
        if (tempTable[playerPos].name === this.nickname) {
            if (position) {
              return playerPos;
            }
            else {
              return tempTable[playerPos].score;
            }
        }
      }
    },

    /*
     *  Get Next Question function
     *  This function simply gets the next question by iterating the player's question index that is locally stored. It updates the data in the Player table and then loads the new question.
     */
    getNextQuestion() {
      this.currQuestion++;
      this.updatePlayerTable();
        this.loadQs(false);
    },

    /*
     *  Get Questions from Open Trivia DB API function
     *  This function loads the questions from either the Open Trivia DB or from the Database.
     *  If it loads them from the Open Trivia DB, then it would save them locally and then update the database with the new questions so other players can play using the same question.
     *  Otherwise, it will just continue to load the question the player is currently on. (as the questions will be stored in the lobbyInfo object pulled from the DB)
     */
    async getQs() {
      let noData = false;
      let response = await fetch(`/quizApi/Questions/inlobby/${this.lobbyInfo.id}`).then((res) => res.json()).catch((error) => noData = true);
      if (noData) { // checks the DB lobby data to see if questions have already been generated, if not, it generates them
        let easyQSelector = this.getRandomNum(10,30);   // pick a random category from the list
        let medQSelector = this.getRandomNum(10,30);
        while (easyQSelector == 29 || easyQSelector == 20 ) { // while the category is not Mythology or General Knowledge
          easyQSelector = this.getRandomNum(10,30);   // pick a different category
        }
        while (medQSelector == 29 || medQSelector == 20 || medQSelector == easyQSelector) { // does the same as above, but makes sure that the category is different to above.
          medQSelector = this.getRandomNum(10,30);
        }

        this.allQuestions.push.apply(this.allQuestions, await fetch(`/getQuestions/amount=6&category=${easyQSelector}&difficulty=easy&type=multiple&encode=base64`).then((res) => res.json()).then((res) => res.results)); // using the tokens to get the questions via the API
        this.allQuestions.push.apply(this.allQuestions, await fetch(`/getQuestions/amount=8&category=${medQSelector}&difficulty=medium&type=multiple&encode=base64`).then((res) => res.json()).then((res) => res.results)); // using the tokens to get the questions via the API
        this.allQuestions.push.apply(this.allQuestions, await fetch(`/getQuestions/amount=6&category=9&difficulty=hard&type=multiple&encode=base64`).then((res) => res.json()).then((res) => res.results)); // using the tokens to get the questions via the API
        this.sendQuestions();

        this.decodeJsonData(true);
      }
      else {  // if not, we just need to decode them!
        this.allQuestions = response;
        this.decodeJsonData(false);
      }
      this.loadQs(true);
    },

    /*
     *  Send Questions to the Question table function
     *  This function is responsible for inserting the new questions for a lobby into the question table
     */
    async sendQuestions() {
      let request = this.reformatQuestions(this.allQuestions);
      const response = await fetch(`/quizApi/Questions/list`, {                 // the POST request to push our array of Question objects
        method: 'POST',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(request)
      });
    },
    /*
     *  Reformat Questions into the API format
     *  This function takes the data that the Open Trivia DB sends and converts it into a format that the API understands
     */
    reformatQuestions(questionArr) {
      let result = [];                                                                                                                     // this is the data structure that will be sending the questions to the API
      let questionIndex = 1;                                                    // used to include question numbers with each questions
      let question;                                                             // used in the for loop to iterate through the tempQuestions array
      let reqQuestion;                                                          // used in the for loop to temporarily store the question in the accepted format for the API
      for (question of questionArr) {
        reqQuestion = question;
        reqQuestion.questionIndex = questionIndex;
        questionIndex++;
        reqQuestion.lobbyId = this.lobbyInfo.id;                                // save the question number and correct answer to each question as well as the lobby ID
        reqQuestion.correctAnswer = question.correct_answer;

        for (let i = 0; i < reqQuestion.incorrect_answers.length; i++) {        // loops through the incorrect answer array and saves them as individual variables for the API to understand
          let propertyName = "incorrectAnswer" + (i+1);
          reqQuestion[propertyName] = reqQuestion.incorrect_answers[i];
        }
        delete(reqQuestion.correct_answer);
        delete(reqQuestion.incorrect_answers);
        result.push(reqQuestion);
      }
      return result;
    },
    /*
     *  Decode JSON Data function
     *  This function is responsible for decoding the Base64. It is run after we pull the questions from the DB, so it decodes each question and returns it
     *  to it's original format for the frontend to read and process easily.
     */
    decodeJsonData(newData) {  // had issues with HTML encoding so this converts the Base64 encoded data back into ASCII
      let question;
      if (newData) {  // decoding data from Open Trivia API
        for (question of this.allQuestions) {
          question.category = decodeURIComponent(escape(window.atob(question.category)));
          question.correct_answer = decodeURIComponent(escape(window.atob(question.correctAnswer)));
          question.difficulty = decodeURIComponent(escape(window.atob(question.difficulty)));
          question.question = decodeURIComponent(escape(window.atob(question.question)));
          question.type = decodeURIComponent(escape(window.atob(question.type)));
          question.incorrect_answers = [];
          for (let i = 0; i < 3; i++) {
            let incAnswerString = `incorrectAnswer${i+1}`;
            question.incorrect_answers[i] = decodeURIComponent(escape(window.atob(question[incAnswerString])));
          }
        }
      }
      else {  // decoding data from DB
        for (question of this.allQuestions) {
          question.category = decodeURIComponent(escape(window.atob(question.category)));
          question.correct_answer = decodeURIComponent(escape(window.atob(question.correctAnswer)));
          question.difficulty = decodeURIComponent(escape(window.atob(question.difficulty)));
          question.question = decodeURIComponent(escape(window.atob(question.question)));
          question.type = decodeURIComponent(escape(window.atob(question.type)));
          delete question.correctAnswer;
          let incorrectAnswers = [];
          let answer;
          for (let i = 0; i < 3; i++) {
            let propertyName = "incorrectAnswer" + (i+1);
            incorrectAnswers.push(decodeURIComponent(escape(window.atob(question[propertyName]))));
            delete question[propertyName];
          }
          question.incorrect_answers = incorrectAnswers;
        }

      }
    },

    /*
     *  Load Current Question function
     *  This function loads the current question - so it loads the data into the currQuestionJSON object from the lobbyInfo object.
     *  It decodes the question data, updates the question field as well as the difficulty, the topic and the question number fields.
     */
    async loadQs(firstLoad) {
      if (firstLoad) {  // if it's the first time that it's loading the game
        let request = {
          name: this.nickname,
          lobbyId: this.lobbyInfo.id
        };
        let playerInfo = await fetch(`/quizApi/Players/getInfo`, {                 // the POST request to get the player info so we can find the question the player is up to
          method: 'POST',
          headers: {
            'Content-Type': 'application/json;charset=utf-8'
          },
          body: JSON.stringify(request)
        }).then((res) => res.json());
        this.currQuestion = playerInfo.questionIndex;
        this.lifeline5050 = playerInfo.lifeline5050;
        this.lifelineSkip = playerInfo.lifelineSkip;
      }
      if (this.currQuestion == 20) { // TODO: end of game
        const allAnsButtons = document.getElementsByClassName("answerButton");
        let ansButton;
        for (ansButton of allAnsButtons) {
          ansButton.disabled = true;
        }
        let playerString;
        if (firstLoad) {
          playerString = `nickname=${this.nickname};lobbyId=${this.lobbyInfo.id};firstLoad=true`;
        }
        else {
          playerString = `nickname=${this.nickname};lobbyId=${this.lobbyInfo.id};firstLoad=false`;
        }
        this.$router.push({ name: 'results', params: { playerInfo: playerString} });
        return true;
      }
      if(this.lifeline5050 == false) {
        document.getElementById("fiftyfifty").disabled = true; //if lifeline has already been used, disable the button
      }
      if(this.lifelineSkip == false) {
        document.getElementById("skip").disabled = true; //if lifeline has already been used, disable the button
      }
      document.getElementById("questionNumber").innerHTML = `Question ${this.currQuestion+1}`;  // updates the question number and the topic
      document.getElementById("questionTopic").innerHTML = `Difficulty: ${this.allQuestions[this.currQuestion].difficulty} <br>Topic: ${this.allQuestions[this.currQuestion].category} </br>`
      document.getElementById("questionBox").innerHTML = this.allQuestions[this.currQuestion].question;  // updates the question box and shows the question to the player
      this.updateQuestion();
    },

    /*
     *  Update Question Buttons function
     *  This function updates the answer buttons by randomly selecting one of them to be the correct answer and then populating the remaining buttons with incorrect answers
     */
    updateQuestion() { // pick a random number between 1 and 4, this will be used to asign the correct answer to a button.
      const correctAnswer = this.getRandomNum(0,3);
      const answerLabels = document.getElementsByClassName("answerLabel");
      answerLabels[correctAnswer].innerHTML = this.allQuestions[this.currQuestion].correct_answer;
      let counter = 0;
      for (let i = 0; i < answerLabels.length; i++) { // assigns the other answers to the remaining buttons
        if (answerLabels[i].innerHTML != this.allQuestions[this.currQuestion].correct_answer) {
          answerLabels[i].innerHTML = this.allQuestions[this.currQuestion].incorrect_answers[counter];
          counter++;
        }
      }
      this.startTimer();
    },

    /*
     *  Get a random number function
     *  This simply generates a random number between the given range and returns it.
     *  SOURCE: https://www.freecodecamp.org/news/how-to-use-javascript-math-random-as-a-random-number-generator/
     */
    getRandomNum (min, max) {
      return Math.floor(Math.random() * (max - min + 1)) + min;
    },

    /*
     *  When Timer Runs Out Function
     *  If player has not answered q before timer runs out, deduct 500 points, set streak to 0, load next q
     */
    onTimesUp() {
      clearInterval(this.timerInterval);
      this.updatePlayerScoreAndPos(-500);
      this.getNextQuestion();
      this.streak=0;
    },

    /*
     *  Start Timer Function
     *  Sets time passed to 0, clears the interval and restarts the timer, called when a new question is loaded
     */
    startTimer() {
      this.timePassed = 0;
      clearInterval(this.timerInterval);
      this.timerInterval = setInterval(() => (this.timePassed += 1), 1000);
    }

  },
  computed: {

    /*
     *  Function to animate the countdown
     */
    circleDasharray() {
      return `${(this.timeFraction * FULL_DASH_ARRAY).toFixed(0)} 283`;
    },

    /*
     *  Function to format the timer
     */
    formattedTimeLeft() {
      const timeLeft = this.timeLeft;
      const minutes = Math.floor(timeLeft / 60);
      let seconds = timeLeft % 60;

      if (seconds < 10) {
        seconds = `0${seconds}`; //If timer has <10s to go, prefix with a 0
      }

      return `${minutes}:${seconds}`;
    },

    /*
     *  Function to return the time left on the timer
     */
    timeLeft() {
      return TIME_LIMIT - this.timePassed;
    },

    /*
     *  Function to reduce the length of the ring gradually during the countdown to ensure animation reaches 0
     */
    timeFraction() {
      const rawTimeFraction = this.timeLeft / TIME_LIMIT;
      return rawTimeFraction - (1 / TIME_LIMIT) * (1 - rawTimeFraction);
    },

    /*
     *  Function to return the correct colour for the countdown animation
     */
    remainingPathColor() {
      const { alert, warning, info } = COLOR_CODES;

      if (this.timeLeft <= alert.threshold) {
        return alert.color;
      } else if (this.timeLeft <= warning.threshold) {
        return warning.color;
      } else {
        return info.color;
      }
    }
  },

  watch: {

    /*
     *  Function that calls onTimesUp() when timer reaches 0
     */
    timeLeft(newValue) {
      if (newValue === 0) {
        this.onTimesUp();
      }
    }
  },

}

</script>


<style lang="scss">

  .answerLabel {
      white-space: break-spaces;
  }
  .answerButton {
    border-color: black !important;
  }

  .answerButton:disabled {
    opacity: 1;
  }

  .border{
    border: 3px solid black;
  }

  .tile{
    width: 100%;
  }

  .box {
    padding: 1rem;
    height: auto;
  }

  .is-yellow{
    background-color: #ffba49;
    color: black;
    text-align: center;
  }

  .lifeline{
    width: 170px;
    height: 100px;
    font-size: 25px;;
  }

  .is-centered{
    text-align: center;
  }

    .base-timer {
  position: relative;
  width: 100px;
  height: 100px;
  z-index: 2;
  background-color: white;
  display: flex;
  float: right;
  top: 9rem;

  &__svg {
    transform: scaleX(-1);
  }

  &__circle {
    fill: none;
    stroke: none;
  }

  &__path-elapsed {
    stroke-width: 7px;
    stroke: grey;
  }

  &__path-remaining {
    stroke-width: 7px;
    stroke-linecap: round;
    transform: rotate(90deg);
    transform-origin: center;
    transition: 1s linear all;
    fill-rule: nonzero;
    stroke: currentColor;

    &.green {
      color: rgb(65, 184, 131);
    }

    &.orange {
      color: orange;
    }

    &.red {
      color: red;
    }
  }

  #timerLabel {
    position: absolute;
    width: 100px;
    height: 100px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 40px;
  }
  #questionTopic {
    display: flex;
  }
}
</style>
