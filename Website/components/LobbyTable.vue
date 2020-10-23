<template>
    <div>
        <b-table class="playerTable" :data="info" :columns="columns"></b-table>
    </div>
    <!--<div>
        <h1>{{ info }}</h1>
    </div>-->
</template>

<script>
    /*function displayResult()
    {
        var index = document.getElementById("playerTable").rows.length;
        var new_row = '<td>'+index+'</td><td>cell 1</td><td>cell 2</td>';
        document.getElementById("playerTable").insertRow(-1).innerHTML = new_row;
    }
    var index = 1;
    this.position = index;*/
    /*var i;
    var index;
    for (i = 0; i < document.getElementById("playerTable").rows.length; i++) {
        index+1;
    }*/
    export default {
        data() {
            return {
                info: [],
                columns: [
                    /*{
                        field: 'index',
                        label: 'Position',
                        width: '50',
                        numeric: true,
                        centered: true,
                    },*/
                    {
                        field: 'name',
                        label: 'Nickname',
                        width: '10',
                        centered: true,
                    },
                    {
                        field: 'score',
                        label: 'Score',
                        width: '400',
                        centered: true,
                        sortable: true,
                    }
                ]
            }
        },
        async fetch() {
          this.info = await fetch(`/quizApi/Players`)
            .then((res) => res.json())
          console.log(this.info);
        }
    }
    /*export default {
        data() {
            return {

                data: [
                    { 'id': 1, 'nickname': 'Jesse'},
                    { 'id': 2, 'nickname': 'John'},
                    { 'id': 3, 'nickname': 'Tina'},
                    { 'id': 4, 'nickname': 'Clarence'},
                    { 'id': 5, 'nickname': 'Anne'}
                ],
                columns: [
                    {
                        field: 'id',
                        label: 'ID',
                        width: '50',
                        numeric: true,
                        sticky: true
                    },
                    {
                        field: 'nickname',
                        label: 'Nickname',
                        width: '500',
                        sticky: true
                    }
                ]
            }
        }
    }*/
</script>

<style>
.playerTable {
    max-height: 350px;
    overflow-y: auto;
    font-size: 25px;
}
.playerTable td {
    padding: 15px;
}
.playerTable th {
    color: white !important;
    background-color: black;
    position: sticky;
    top: 0;
    z-index:999;
    padding: 20px;
}
</style>
