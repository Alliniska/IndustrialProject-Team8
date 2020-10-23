<template>
    <div>
        <div class="box">
            <article class="content">
                <h2>Quizz.io</h2>
            </article>
        </div>
        
        <div class="menu">
            <nuxt-link to="/howtoplay"><b-button class="howtoplay"> How To Play </b-button></nuxt-link>
            <nuxt-link to="/about"><b-button class="about"> About </b-button></nuxt-link>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Header",
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
.box
{
    display: table-cell;
    padding: 0px;
    top: 48px;
    left: 355px;
    width: 950px;
    height: 188px;
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
.menu 
{
    font-size: 0rem;
    text-align: center;
}
.howtoplay
{
    opacity: 1;
    font: 32px Rockwell;
    border-radius: 0px;
    border: 3px solid #000000;
    background: #24011E 0% 0% no-repeat padding-box;
    box-shadow: 0px 3px 6px #00000029;
    letter-spacing: 0px;
    color: #FFFFFF;
    text-align: center;
    width: 35%;
}
.about
{
    opacity: 1;
    font: 32px Rockwell;
    border-radius: 0px;
    border: 3px solid #000000;
    background: #24011E 0% 0% no-repeat padding-box;
    box-shadow: 0px 3px 6px #00000029;
    letter-spacing: 0px;
    color: #FFFFFF;
    text-align: center;
    width: 35%;
}
*, *::before, *::after {
    box-sizing: border-box;
}
</style>
