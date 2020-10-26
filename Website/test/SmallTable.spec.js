import { shallowMount } from '@vue/test-utils'
import SmallTable from '@/components/SmallTable.vue'

describe('Testing the SmallTable component', () => {
    
    it('SmallTable is a vue instance', () => {
        const wrapper = shallowMount(SmallTable,{
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