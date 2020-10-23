<template>
    <section>
        <div class="tile">
            <article class="tile is-child notification is-black">
                <p class="title">{{ title }}</p>
                <p class="subtitle">{{ msg }}</p>
            </article>
        </div>
    </section>
</template>

<script>
export default {
  props: {
    title: {
      type: String,
      required: false
    },
    msg: {
      type: String,
      required: false
    }
  }
}
</script>


<style>
.is-black {
  background-color: black;
}
</style>
