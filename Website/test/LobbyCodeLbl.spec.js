import { shallowMount } from '@vue/test-utils'
import LobbyCodeLbl from '@/components/LobbyCodeLbl.vue'

describe('Testing the LobbyCodeLbl component', () => {
    
    it('LobbyCodeLbl is a vue instance', () => {
        const wrapper = shallowMount(LobbyCodeLbl,{
            props: 
            {
                title: 
                {
                  type: String,
                  required: false
                },
                msg: 
                {
                  type: String,
                  required: false
                }
            }
        });
        expect(wrapper.vm).toBeTruthy()
    });

    it("Test if title is added to title", () => {
        const wrapper = shallowMount(LobbyCodeLbl,{
            props: 
            {
                title: 
                {
                  type: String,
                  required: false
                },
                msg: 
                {
                  type: String,
                  required: false
                }
            }
          });
          wrapper.setData({ title: 'title_test'});
          expect(wrapper.vm.title).toBe('title_test');
        });

        it("Test if title is added to tile", () => {
            const wrapper = shallowMount(LobbyCodeLbl,{
                props: 
                {
                    title: 
                    {
                      type: String,
                      required: false
                    },
                    msg: 
                    {
                      type: String,
                      required: false
                    }
                }
              });
              wrapper.setData({ msg: 'test_message'});
              expect(wrapper.vm.msg).toBe('test_message');
            });
});