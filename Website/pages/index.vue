<template>
  <div>
    <!--<GameMusic />-->
    <section class="section">
      <div class="columns is-mobile">
        <!--<Header></Header>-->
      </div>
      <!-- <Button title="How To Play"></Button>
      <Button title="About"></Button> -->
    </section>
    <section class="nickname">
       <b-field>
         <b-input placeholder="Enter Nickname" size="is-large" icon="account" rounded id="nicknameInput"></b-input>
       </b-field>
       <b-field style="visibility:hidden" id="lobbyField">
         <b-input placeholder="Enter Lobby ID" size="is-large" icon="account" rounded id="LobbyIDInput"></b-input>
       </b-field>
       <div class="buttons is-centered lobbyBtns">
        <BlackButton @click.native="getLobbyCode" title="Join Lobby" ></BlackButton>
        <BlackButton @click.native="storeNickname" title="Create Lobby" id="createButton"></BlackButton>
       </div>
    </section>
    <div class="container " id="errorNotifContainer">
      <b-notification
              type="is-danger"
              has-icon
              auto-close
              aria-close-label="Close notification"
              v-model="notifDisplay"
              role="alert"
              id="errorNotif">
        </b-notification>

    </div>
  </div>
</template>

<script>
import Header from '~/components/Header'
import BlackButton from '~/components/BlackButton'
import router from '../router'

export default {
  name: 'home',
  components: {
    Header,
    BlackButton
  },
  data() {
    return {
      nickname: null,
      notifDisplay: false,
      playerExistsAlready: false,
      isComponentModalActive: false,
      lobbyId: 0,
    }
  },
  methods: {
      /*
       *  the Store Nickname function is called when creating a new lobby
       *  It has some basic error checking to make sure that the nickname field isn't empty
       */
    storeNickname() {
      if (document.getElementById("nicknameInput").value == "") { // if there is nothing in the nickname field
        this.notifDisplay = true;   // show the no nickname warning.
      }
      else {  // if there is something in the nickname field, then ask for the lobby ID
        let nicknameVar = `nickname=${document.getElementById("nicknameInput").value};lobbyId=0;playerExists=${this.playerExistsAlready}`;
        this.$router.push({ name: 'lobby', params: {playerInfo: nicknameVar}});
      }
    },

    /*
     *  the Run Lobby Checks function is called as a part of the error checking statements
     *  it is responsible for making sure that the lobby already exists in the database and it also flags if the player is already in the lobby
     */
    async runLobbyChecks(lobbyId, nickname) {
      let response = await fetch(`/quizApi/Lobbies/${lobbyId}`);                    // this GET request attempts to get the lobby info from the DB
      if (response.status == 200) {                                                 // if it does exist then ...
        let request = {
          name: nickname,
          lobbyId: parseInt(lobbyId)
        }
        let checkPlayer = await fetch(`/quizApi/Players/getInfo`, {                 // ... we send a post request to see if the player exists in the lobby!
          method: 'POST',
          headers: {
            'Content-Type': 'application/json;charset=utf-8'
          },
          body: JSON.stringify(request)
        });
        if (checkPlayer.status == 200) {                                           // if a player with that name already exists then ....
          this.playerExistsAlready = true;
          return true;                                                             // we return true and set the flag
        }
        else {                                                                     // otherwise we ignore it and return true anyway
          return true;
        }
      }
      else {                                                                       // if the lobby isn't in the database, it will show this error and not proceed to the next pa
        document.getElementById("errorNotif").innerHTML = "The lobby doesn't exist. Please check what you have entered or speak to your game host.";
        this.notifDisplay = true;   // show the no nickname warning.
        return false;
      }

    },

    /*
     *  the Get Lobby Code function is called when the player joins an existing lobby
     *  it has error checking for the nickname field and the lobby ID field, ensuring that a nickname and a valid 8 digit lobby ID is entered
     */
    async getLobbyCode() {
      if (document.getElementById("nicknameInput").value == "") { // if there is nothing in the nickname field
        document.getElementById("errorNotif").innerHTML = "Please enter a nickname in the box before clicking on a button.";
        this.notifDisplay = true;   // show the no nickname warning.
      }
      else {  // if there is something in the nickname field, then ask for the lobby ID
        document.getElementById("lobbyField").style.visibility = "visible";
        document.getElementById("createButton").style.visibility = "hidden";
        let lobbyIDinput = document.getElementById("LobbyIDInput").value;
        let nicknameInput = document.getElementById("nicknameInput").value;
        if (lobbyIDinput.length == 8 && !isNaN(lobbyIDinput) && await this.runLobbyChecks(lobbyIDinput, nicknameInput)) { // check if its an 8 digit number
          let playerString = `nickname=${nicknameInput};lobbyId=${lobbyIDinput};playerExists=${this.playerExistsAlready}`;
          this.$router.push({ name: 'lobby', params: {playerInfo: playerString} });
        }
      }
    }
  }
}
</script>

<style>

#errorNotifContainer {
  margin-top: 2rem;
  left: 15rem;
}

#errorNotif {
  width: 50rem;
}

.lobbyBtns {
  margin-top: 3rem;
}

.nickname
{
  width: 60rem;
  margin-top: 5rem;
  margin-left: auto;
  margin-right: auto;
}

</style>
