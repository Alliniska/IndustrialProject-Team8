<template>
    <section class="sound">
        <!-- when audio is ready to be played, start method is called -->
        <audio id="audio" src="~/assets/Quizz.mp3" v-on:canplay="start()" loop></audio>
        <img class="img" id="img" src="~/assets/volume_on.png" @click="toggle()" alt="Toggle Sound" height="80px" width="80px" align="right">
    </section>
</template>

<script>

export default 
{
    name: "GameMusic",

    methods: {
        // this method starts the music and sets the volume
        start()
        {
            var a = document.getElementById("audio")
            a.volume = 0.2
            a.play()
        },
        // this method controls if the music should be paused or played
        toggle() 
        {
            if(!audio.paused)
            {
                audio.pause();
            }
            else
            {
                audio.play();
            }
                
        }
    }
}
</script>

<style>
.img
{
    cursor: pointer;
}
</style>