<template>
    <div>
        <b-table class="playerTable" :data="info" :columns="columns"></b-table>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                info: [],
                columns: [
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
