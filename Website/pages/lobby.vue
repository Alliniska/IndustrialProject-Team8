<template>
    <div class="section is-fullheight">
      <div class="container column is-mobile is-centered is-8">
          <LobbyCodeLbl title="Players"></LobbyCodeLbl>
      </div>
      <div class="container column is-mobile is-centered is-8">
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
      <div class="buttons is-mobile is-centered">
        <div class="is-3">
            <BlackButton @click.native="startGame" title="Start Game"></BlackButton>
        </div>
        <div class="is-3">
          <router-link to="/"><BlackButton title="Leave Game"></BlackButton></router-link>
        </div>
      </div>
    </div>
</template>

<script>
export default {
  name: 'lobby',
  metaInfo: {
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' }
    ]
  },
  data() {
    return {
        tableData: [],
        nickname: "",
        tableColumns: [
            {
                field: 'name',
                label: 'Nickname',
                width: '150',
            },
            {
                field: 'score',
                label: 'Score',
                numeric: true,
                sortable: true,
                centered: true,
                width: '100'
            }
        ],
      lobbyInfo: []
    }
  },
  /*
   *  this is the first subroutine called - it handles the route parameters, it gets them and stores them in the parent object
   *  it also checks to see if it needs to make a lobby or add the player to an existing lobby
   */
  created() {
    let playerInfo = this.$route.params.playerInfo;
    let tempPlayerInfo = playerInfo.split(";");
    this.nickname = tempPlayerInfo[0].split("nickname=")[1];
    let lobbyId = tempPlayerInfo[1].split("lobbyId=")[1];
    let playerExistsAlready = tempPlayerInfo[2].split("playerExists=")[1];
    document.getElementById("userNickname").innerHTML = this.nickname;
    if (lobbyId == 0) {
      this.makeLobby();
    }
    else {
      if (playerExistsAlready == "true") {
        this.refreshLeaderboard(lobbyId);
      }
      else {
        this.addPlayerToLobby(lobbyId);
      }
      this.getLobbyInfo(lobbyId);
    }
    document.getElementById("lobbyCode").innerHTML = lobbyId;
  },
  methods: {

     /*
      *   the Start Game function is called when the player wants to start playing the game
      *   essentially, it gets their data and forwards it to the game page
      */
     startGame() {
       let playerString = `nickname=${this.nickname};lobbyId=${this.lobbyInfo.id}`;
       this.$router.push({ name: 'game', params: { playerInfo: playerString} });
     },

     /*
      *   the Get Lobby Info function is called so that the information about the lobby is stored in a property in the parent object
      */
     async getLobbyInfo(id) {
       this.lobbyInfo = await fetch(`/quizApi/Lobbies/${id}`).then((res) => res.json());
       return true;
     },

     /*
      *   the Make Lobby function is called in the created function to generate a lobby ID to display to the player
      *   it sends a request to the API which generates an ID and stores it in the database.
      *   Once the lobby has been made, it adds the player to it.
      */
     async makeLobby() {
       let request = {
         requestURL: "DEPRECATED",
         date: new Date().toISOString()
       };
       this.lobbyInfo = await fetch(`/quizApi/Lobbies`, {                 // the POST request to generate a new lobby and get the ID for it.
         method: 'POST',
         headers: {
           'Content-Type': 'application/json;charset=utf-8'
         },
         body: JSON.stringify(request)
       }).then((res) => res.json());
       document.getElementById("lobbyCode").innerHTML = this.lobbyInfo.id;
       this.addPlayerToLobby(this.lobbyInfo.id);
     },

     /*
      *   the Add Player to Lobby function is responsible for taking the lobby ID and using that to add the player to a specific lobby
      *   The player is given initialised values and added to the lobby - once added, the leaderboard is refreshed to show the player on it.
      */
     async addPlayerToLobby(lobbyId) {
       this.lobbyInfo = await fetch(`/quizApi/Lobbies/${lobbyId}`).then((res) => res.json());
       let request = {
         name: this.nickname,
         score: 0,
         lobbyId: parseInt(lobbyId),
         questionIndex: 0,
         lifeline5050: true,
         lifelineSkip: true
       };
       let response = await fetch(`/quizApi/Players`, {                 // the POST request to generate a new lobby and get the ID for it.
         method: 'POST',
         headers: {
           'Content-Type': 'application/json;charset=utf-8'
         },
         body: JSON.stringify(request)
       });
       this.refreshLeaderboard(lobbyId);
     },

     /*
      *   the Refresh Leaderboard function essentially gets the latest version of the leaderboard for the lobby from the database.
      */
      async refreshLeaderboard(lobbyId) {
        this.tableData = await fetch(`/quizApi/Players/inlobby/${lobbyId}`).then((res) => res.json());
      }
  },
}
</script>
