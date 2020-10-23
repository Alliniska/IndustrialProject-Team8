export default {
  // Disable server-side rendering (https://go.nuxtjs.dev/ssr-mode)
  ssr: false,

  // Target (https://go.nuxtjs.dev/config-target)
  target: 'server',

  // Global page headers (https://go.nuxtjs.dev/config-head)
  head: {
    title: 'Quizz.io',
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: '' }
    ],
    link: [
      { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }
    ]
  },

  // Global CSS (https://go.nuxtjs.dev/config-css)
  css: [
  ],

  // Plugins to run before rendering page (https://go.nuxtjs.dev/config-plugins)
  plugins: [
  ],

  // Auto import components (https://go.nuxtjs.dev/config-components)
  components: true,

  // Modules for dev and build (recommended) (https://go.nuxtjs.dev/config-modules)
  buildModules: [
  ],

  // Modules (https://go.nuxtjs.dev/config-modules)
  modules: [
    // https://go.nuxtjs.dev/buefy
    'nuxt-buefy',
    '@nuxt/http',

  ],

  http: {
      // proxyHeaders: false
      proxy: true
  },

  proxy: {
    '/quizApi/': {
      target: 'https://riddlersquizapiv2.azurewebsites.net',
      pathRewrite: { '^/quizApi/': '/api/' }
    },
    '/getQToken/': {
      target: 'https://opentdb.com',
      pathRewrite: { '^/getQToken/': '/api_token.php?command=request'}
    },
    '/getQuestions/': {
      target: 'https://opentdb.com',
      pathRewrite: { '^/getQuestions/': '/api.php?'}
    }
  },
  // Build Configuration (https://go.nuxtjs.dev/config-build)
  build: {
    loaders:
    {
      vue:
      {
        transformAssetUrls:
        {
          audio: 'src'
        }
      }

    },
    extend(config, ctx) {
      config.module.rules.push({
        test: /\.(ogg|mp3|wav|mpe?g)$/i,
        loader: 'file-loader',
        options: {
          name: '[path][name].[ext]'
        }
      })
    }
  }
}
