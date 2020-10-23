<template>
  <div class="container">
    <div class="tile is-ancestor">
      <div class="tile is-parent is-8">
        <article class="tile is-child box border"><!-- QUESTION TILE -->
          <p class="title" id="questionNumber">Question 000</p>
          <p class="subtitle" id="questionTopic">Topic: ---</p>
          <div class="content">
            <p class="is-size-3-tablet" id="questionBox"></p>
          </div>
        </article>
      </div>
      <div class="tile is-parent is-vertical"> <!-- SCORE AND LIFELINE TILES -->
        <article class="tile is-child box border">
          <p class="title centered">Lifelines</p>
          <p class="subtitle">Need some help? This is the place!</p>
          <div class="buttons">
            <b-button class="lifeline is-yellow">50/50</b-button>
            <b-button class="lifeline is-yellow">Skip Question</b-button>
          </div>
        </article>
        <article class="tile is-child box border">
          <p class="title" id="playerPosition">2nd</p>
          <p class="subtitle" id="playerScore">Score: 3000</p>
        </article>
      </div>
    </div>
    <div class="tile is-ancestor">
      <div class="tile is-parent is-vertical buttons">
        <b-button @click="checkAnswer('ansOne')" class="tile is-size-3-tablet is-child field is-grouped is-white">
          <p class="is-size-3-tablet answerLabel" id="ansOne">----</p>
        </b-button>
        <b-button @click="checkAnswer('ansTwo')" class="tile is-size-3-tablet is-child field is-grouped is-white">
          <p class="is-size-3-tablet answerLabel" id="ansTwo">----</p>
        </b-button>
      </div>
      <div class="tile is-parent is-vertical buttons">
        <b-button @click="checkAnswer('ansThree')" class="tile is-size-3-tablet is-child field is-grouped is-white">
          <p class="is-size-3-tablet answerLabel" id="ansThree">----</p>
        </b-button>
        <b-button @click="checkAnswer('ansFour')" class="tile is-size-3-tablet is-child field is-grouped is-white">
          <p class="is-size-3-tablet answerLabel" id="ansFour">----</p>
        </b-button>
      </div>
      <div class="tile is-parent is-4"> <!-- LEADERBOARD TILE -->
        <article class="tile is-child box border">
          <p class="subtitle">Wondering how your competitors are doing? This is the place for you 👀</p>
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
                default-sort="score"
                default-sort-direction="desc"
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
import SmallTable from '~/components/SmallTable'
export default {
  name: 'Game',
  data() {
    return {
        info: [],
        currQuestion: 1,
        currQuestionJSON: null,
        userLobbyId: 90909090,
        nickname: "felicia",
        score: 2300,
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
      easyQs: [],
      mediumQs: [],
      hardQs: []
    }
  },
  methods: {
    startGame() {
      console.log("YAY");
      this.getNewQs();
    },
    checkAnswer(buttonId) {
      console.info(document.getElementById(buttonId).innerHTML)
      if (document.getElementById(buttonId).innerHTML == this.currQuestionJSON.correct_answer) {
        alert("Correct answer! ✔");
        this.updatePlayerScoreAndPos(500);
      }
      else {
        alert("Wrong answer! ❌");
        this.updatePlayerScoreAndPos(-500);
      }
      this.getNextQuestion();
    },
    updatePlayerScoreAndPos(adjustment) {
      this.score += adjustment;
      document.getElementById("playerScore").innerHTML = `Score: ${this.score}`;
      this.refreshLeaderboard();
      this.tableData.sort(function (a,b) {
        return b.score - a.score;
      });
      let playerPos = 0;
      for (playerPos=0; playerPos < this.tableData.length; playerPos++) {
        console.info("in player loop!");
        if (this.tableData[playerPos].name === this.nickname) {
            break;
        }
      }
      playerPos++;
      let positionElem = document.getElementById("playerPosition");
      console.info(playerPos);
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
      // TODO: Update the player score in the database HERE
    },
    getNextQuestion() {
      this.currQuestion++;
      // TODO: Update the question index in the database HERE
      this.loadQs();
    },
    async refreshLeaderboard() {
      this.tableData = await fetch(`/quizApi/Players/inlobby/${this.userLobbyId}`).then((res) => res.json());
      console.info(this.tableData);
    },
    async getExistingQs() {  // for existing lobbies
      this.lobbyInfo = await fetch(`/quizApi/Lobbies/${this.userLobbyId}`).then((res) => res.json());
      this.loadQs();
    },
    async getNewQs() { // for new lobbies only
      this.easyQs = await fetch(`/getQuestions/amount=10&category=9&difficulty=easy&type=multiple`).then((res) => res.json()).then((res) => res.results); // using the tokens to get the questions via the API
      this.mediumQs = await fetch(`/getQuestions/amount=10&category=9&difficulty=medium&type=multiple`).then((res) => res.json()).then((res) => res.results);
      this.hardQs = await fetch(`/getQuestions/amount=10&category=9&difficulty=hard&type=multiple`).then((res) => res.json()).then((res) => res.results);
      console.info(this.easyQs);
      console.info(this.mediumQs);
      console.info(this.hardQs);
      // this.createLobby(); // sending the tokens to the API for storage in the DB.
      this.loadQs();
    },
    loadQs() {
      console.info(this.currQuestion - 1);
      if (this.currQuestion <= 10) {
        this.currQuestionJSON = this.easyQs[this.currQuestion -1];
      }
      else if (this.currQuestion <= 20 && this.currQuestion > 10) {
        this.currQuestionJSON = this.mediumQs[this.currQuestion -1];
      }
      else if (this.currQuestion <= 30 && this.currQuestion > 20) {
        this.currQuestionJSON = this.hardQs[this.currQuestion -1];
      }
      console.info(this.currQuestionJSON);
      document.getElementById("questionNumber").innerHTML = `Question ${this.currQuestion}`;  // updates the question number and the topic
      document.getElementById("questionTopic").innerHTML = `Topic: ${this.currQuestionJSON.category}`
      this.updateQuestion();
    },
    updateQuestion() { // pick a random number between 1 and 4, this will be used to asign the correct answer to a button.
      document.getElementById("questionBox").innerHTML = this.currQuestionJSON.question;  // updates the question box and shows the question to the player
      const correctAnswer = this.getRandomNum(0,3);
      const answerLabels = document.getElementsByClassName("answerLabel");
      answerLabels[correctAnswer].innerHTML = this.currQuestionJSON.correct_answer;
      let counter = 0;
      for (let i = 0; i < answerLabels.length; i++) { // assigns the other answers to the remaining buttons
        if (answerLabels[i].innerHTML != this.currQuestionJSON.correct_answer) {
          answerLabels[i].innerHTML = this.currQuestionJSON.incorrect_answers[counter];
          counter++;
        }
      }
    },
    getRandomNum (min, max) { // SOURCE: https://www.freecodecamp.org/news/how-to-use-javascript-math-random-as-a-random-number-generator/
      return Math.floor(Math.random() * (max - min + 1)) + min;
    },
    async createLobby() {
      const date = new Date();
      const newLobby = {
        id: this.userLobbyId,
        easyQs: JSON.stringify(this.easyQs),
        mediumQs: JSON.stringify(this.mediumQs),
        hardQs: JSON.stringify(this.hardQs),
        date: date.toISOString(),
        requestURL: "amount=10&category=9&type=multiple"
      };
      const response = await fetch('/quizApi/Lobbies', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(newLobby)
      });
    }
  },
  async fetch() {
    this.lobbyInfo = await fetch(`/quizApi/Lobbies/${this.userLobbyId}`).then((res) => res.json());
    console.info(JSON.stringify(this.lobbyInfo));
    document.getElementById("lobbyCode").innerHTML = this.userLobbyId;
    document.getElementById("userNickname").innerHTML = this.nickname;
    this.refreshLeaderboard();
    this.startGame();
  },
}

</script>

<style lang="scss">
  $warning: #ffba49;
  $link: #20a39e;
  $info: #a4a9ad;
    $danger: #f9b1b1;
  $primary: #23001e;
  $yellow: #ffba49;

  .border{
    border: 3px solid black;
  }

  .tile{
    width: 100%;
  }

  .is-yellow{
    background-color: #ffba49;
    color: black;
    text-align: center;
  }

  .lifeline{
    width: 164px;
    height: 100px;
    font-size: 25px;;
  }
</style>
