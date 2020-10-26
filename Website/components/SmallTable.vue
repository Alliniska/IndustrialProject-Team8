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
    max-height: 150px;
    overflow-y: auto;
    font-size: 15px;
}
.playerTable td {
    padding: 5px;
}
.playerTable th {
    color: white !important;
    background-color: black;
    position: sticky;
    top: 0;
    z-index:999;
    padding: 5px;
}
</style>
