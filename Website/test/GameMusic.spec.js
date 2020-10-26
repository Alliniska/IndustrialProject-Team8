import { shallowMount } from '@vue/test-utils'
import GameMusic from '@/components/GameMusic.vue'

describe('Testing the GameMusic component', () => {
    
    it('GameMusic is a vue instance', () => {
        const wrapper = shallowMount(GameMusic);
        expect(wrapper.vm).toBeTruthy()
    });

    it('calls toggle method when image is clicked', () => {
        const wrapper = shallowMount(GameMusic);
        expect(wrapper.find('#img').exists()).toBe(true)
        const img = wrapper.find('#img')
        const spy = spyOn(wrapper.vm, 'toggle')
        img.trigger('click')
        expect(wrapper.vm.toggle).toBeCalled()
    });

    it('calls start method when audio can play', () => {
        const wrapper = shallowMount(GameMusic, {
            props: {
                autoplay: false,
                volume: 0.2,
            },
        });
        expect(wrapper.find('#audio').exists()).toBe(true)
        const audio = wrapper.find('#audio')
        const spy = spyOn(wrapper.vm, 'start')
        audio.trigger('canplay')
        expect(wrapper.vm.start).toBeCalled()
    });

});
