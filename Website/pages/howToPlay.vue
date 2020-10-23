<template>
    <div class="tile notification is-white">
            <article class="content">
                <h2>Quizz.io</h2>
                <div class="media-content">
                    <h3>How To Play</h3>
                    <!-- these are the instructions for the game -->
                    <p>
                        Quizz.io is based on the concept of lobbies. At the start of the game, you will be asked to fill in a nickname for yourself and then given the choice to either create or join a lobby.
                        <br><br>
                        <b>Create Lobby</b>
                        <br>
                        This allows you to create a lobby with a set of 20 randomly generated questions. You can then share the lobby code to get more players to answer those questions. You can also decide to play in the lobby by yourself without inviting anyone else.
                        <br>
                        <b>Join Lobby</b>
                        <br>
                        You can join a lobby after entering a nickname. This lobby will have to be created either by yourself or someone else before you join it. This will allow you to compete against all the players who have played in that lobby.
                        <br><br>
                        <b>Game Screen</b>
                        <br><br>
                        <b>Questions</b>
                        <br>
                        You will be playing a game with 20 questions. The questions get progressively harder as you progress. You get four options for each question. Click on an option which you think is the correct answer for that question.
                        <br>
                        <b>Points</b>
                        <br>
                        You will get points for answering each question correctly. You will get negative points for each question you answered incorrectly. The number of points is based on how quickly you answer the question and your streak multiplier.
                        <br>
                        <b>Streak Multiplier</b>
                        <br>
                        You have a power-up called streak multiplier. This is applied automatically to each question. This multiplies the points you receive for each question by a multiplier. Your multiplier starts with 1 and increases for each consecutive question you answer correctly. If you answer a question incorrectly, the multiplier is reset to 1.
                        <br>
                        <b>Timer</b>
                        <br>
                        You get a 30 second timer for each question. If you don’t answer the question within the 30 seconds, you will get negative points for that question and your multiplier will be reset to 1.
                        <br>
                        <b>Lifelines</b>
                        <br>
                        You get two lifelines which you can use once any time during your 20 questions. They are 50/50 and Skip.
                        <br>
                        <b>50/50</b>
                        <br>
                        Using this lifeline eliminates 2 incorrect options from your 4 given options
                        <br>
                        <b>Skip</b>
                        <br>
                        This skips the current question and gives you 0 points but keeps your multiplier intact
                        <br>
                        <b>Leaderboard</b>
                        <br>
                        The leaderboard shows all the players who have played or are playing in the current lobby. The leaderboard is updated at the end of every question.
                    </p>
                    <br>
                    <!-- back button takes you to index.vue -->
                    <nuxt-link to="/"><BlackButton title="Back"></BlackButton></nuxt-link>
                </div>
            </article>
    </div>
</template>

<script>
    import BlackButton from '~/components/BlackButton'
    
    export default {
        components: 
            {
                BlackButton
            }
    }
</script>

<style>
.columns.is-mobile
{
    justify-content: center;
}
.section
{
    padding: 1.5rem 1.5rem;
}
.howtoplaybox
{
    width: 950px;
    height: 188px;
    display: table;
}
.content
{
    margin: 0px;
    justify-content: center;
    text-align: center
}
.content h2 
{
    margin: 0%;
    font: normal normal bold 145px/203px Rockwell;
    letter-spacing: 0px;
    color: #24011E;
}
.media-content
{
    text-align: center;
    margin: 20px 90px;
}
.media-content h3
{
    font: normal normal bold 48px/68px Rockwell;
    letter-spacing: 0px;
    color: #24011E;
    opacity: 1;
}
</style>