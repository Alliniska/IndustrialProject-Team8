import { shallowMount } from '@vue/test-utils'
import BlackButton from '@/components/BlackButton.vue'

describe('Testing the BlackButton component', () => {

  it('BlackButton component is mounted correctly', () => {
    const wrapper = shallowMount(BlackButton,{
      props: {
        title: {
          type: String,
          required: true
        }
      },
        stubs: {
          'b-field': true,
          'b-button': true,
        }
      });
    expect(wrapper.vm).toBeTruthy()
  });

  it("Test if title is added to button", () => {
    const wrapper = shallowMount(BlackButton,{
      props: {
        title: {
          type: String,
          required: true
        }
      },
      stubs: {
        'b-field': true,
        'b-button': true,
      }});
      wrapper.setData({ title: 'test_button'});
      expect(wrapper.vm.title).toBe('test_button');
    });

});
