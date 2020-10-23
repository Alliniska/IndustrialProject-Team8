<template>
    <section>
        <b-field class="Readybutton">
            <b-button type="is-black" size="is-large">{{ title }}</b-button>
        </b-field>
    </section>
</template>

<script>
export default {
  props: {
    title: {
      type: String,
      required: true
    }
  }
}
</script>

<style>
.is-black {
  background-color: black;
}
</style>
