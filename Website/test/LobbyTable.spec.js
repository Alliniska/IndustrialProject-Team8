import { shallowMount } from '@vue/test-utils'
import LobbyTable from '@/components/LobbyTable.vue'

describe('Testing the LobbyTable component', () => {
    
    it('LobbyTable is a vue instance', () => {
        const wrapper = shallowMount(LobbyTable,{
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
                            width: '10',
                            centered: true,
                            sortable: true,
                        }
                    ]
                }
            }
        });
        expect(wrapper.vm).toBeTruthy()
    });
    



    it('LobbyTable is a vue instance', () => {
        const wrapper = shallowMount(LobbyTable,{
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
                            width: '10',
                            centered: true,
                            sortable: true,
                        }
                    ]
                }
            }
        });
        expect(wrapper.vm).toBeTruthy()
    });
});