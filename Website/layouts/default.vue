<template>
  <div class="background">
    <nav class="navbar" role="navigation" aria-label="main navigation">
      <!-- game music is played in this layout so it runs continously while navigating through the site -->

      <GameMusic />
      <!--<div class="columns is-4 is-offset-1 navbar-item">
        <div class="tile">
            <article class="tile is-child notification is-white">
              <div>
                <p class="subtitle">Lobby code:</p>
                <p class="title" id="lobbyCode">----</p>
              </div>
              <div>
                <p class="subtitle">Nickname:</p>
                <p class="title" id="userNickname"> ---- </p>
              </div>
            </article>
        </div>
      </div>-->
      <div class="navbar-brand">
        <a class="navbar-item" href="/">
          <h1 class="is-size-1-desktop title">Quizz.io</h1>
        </a>

          <a class="navbar-item">
            <p class="is-size-4" style="margin-right: 5px">Lobby code: </p>
            <p class="is-size-4" id="lobbyCode">----</p>
          </a>
          <a class="navbar-item">
            <p class="is-size-4" style="margin-right: 5px"> Nickname: </p>
            <p class="is-size-4" id="userNickname">----</p>
          </a>
        </div>
        <div class="navbar-end">
          <div class="buttons is-centered" style="padding-right: 2rem">
            <router-link to="/howToPlay"><b-button class="button" size="is-black is-large">How To Play</b-button></router-link>
            <router-link to="/about"><b-button class="button" size="is-black is-large">About Us</b-button></router-link>
          </div>
        </div>
    </nav>



      <div class="container column is-10">
        <router-view></router-view>
      </div>
  </div>
</template>


<style lang="scss">

.navbar {
  min-height: 6.25rem;
  text-align: center;
  width: 90rem;
  margin: auto;
  background-color: white;
  border: 3px solid black;
  padding: 1rem;
  margin-top: 1rem;
}
html {
  background-image: url("~assets/Background.svg") !important;
  background-color: #20A39E;
  background-position: 0%, 0%, 50%, 50%;
  background-position-y: center;
  background-attachment: fixed;
  background-size: auto;
}
</style>
