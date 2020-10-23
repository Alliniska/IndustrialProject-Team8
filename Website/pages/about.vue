<template>
    <div class="tile notification is-white">
        <article class="content">
            <h2>Quizz.io</h2>
            <div class="media-content">
                <h3>About Us</h3>
                <!-- this is the about content -->
                <p>
                    <br>
                    This game was built by Team 8 for AC41004 - Industrial Team Project.
                    <br>
                    <br>
                    <b>Credits</b>
                    <br>
                    <br>
                    <b>Product Design</b>
                    <br>
                    Amy Gourlay,
                    Renata Jeny,
                    Yousaf Nasir,
                    Mandar Tamhane,
                    Sebastian Thomas
                    <br>
                    <br>
                    <b>Product Development</b>
                    <br>
                    Arnav Dhawan,
                    Kyle Leggate,
                    Dominika Piosik,
                    Steven Marshall
                    <br>
                    Mo Nadat,
                    Amy Gourlay,
                    Sebastian Thomas
                    <br>
                    <br>
                    <b>Project Manager</b>
                    <br>
                    Renata Jeny
                    <br>
                    <br>
                    <b>Questions</b>
                    <br>
                    OpenTrivia Database
                    <br>
                    <br>
                    <b>Music</b>
                    <br>
                    Royalty Free Music from Bensound
                </p>
                <p><br> Made with 💖 © Riddlers 2020</p>
                <br>
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
.aboutbox
{
    width: 950px;
    height: 188px;
    display: table;
}
.content
{
    margin: auto;
    justify-content: center;
    text-align: center;
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
    margin: 20px 50px;
}
.media-content h3
{
    font: normal normal bold 48px/68px Rockwell;
    letter-spacing: 0px;
    color: #24011E;
    opacity: 1;
}
</style>