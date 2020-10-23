<template>
    <div class="tile is-ancestor">
        <div class="tile is-parent">
        <div class="tile is-child box border">
            <p class="title baa titlefont" id="Streak">Your correct answer streak is...</p>
            <p class="subtitle baa subfont" id="timer">Next question in 5s</p>
        </div>
    </div>
    <div class="tile is-4 is-vertical is-parent">
        <div class="tile is-child box border">
            <p class="title baa titlefont" id="playerPosition">2nd</p>
            <p class="subtitle baa subfont" id="playerScore">Score: 3000</p>
        </div>
        <div class="tile is-child is-vertical box border">
          <div class="message">
            <div class="message-header">
              <p>Leaderboard</p>
            </div>
            <div class="message-body">
              <SmallTable></SmallTable>
            </div>
          </div>
        </div>
  </div>
  
</div>
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
    async refreshLeaderboard() {
      this.tableData = await fetch(`/quizApi/Players/inlobby/${this.userLobbyId}`).then((res) => res.json());
      console.info(this.tableData);
    },
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

<style>

  .template{
      height: 100%;
  }

  .baa{
    text-align: center;
  }

  .titlefont{
      font-size: 100px;
  }

  .subfont{
      font-size: 60px
  }

  .border{
      border: 3px solid black;
  }

  .message-header{
      background-color: black;
  }

</style>