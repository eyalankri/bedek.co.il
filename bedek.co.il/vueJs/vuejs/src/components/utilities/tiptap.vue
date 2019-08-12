<template>
  <div class="editor">   
    <editor-menu-bar :editor="editor" style="text-align:left">
     <div slot-scope="{ commands, isActive }">

        <i class="material-icons"  :class="{ 'is-active': isActive.bold() }"
          @click="commands.bold">format_bold</i>

           <i class="material-icons"  :class="{ 'is-active': isActive.italic() }"
          @click="commands.italic">format_italic</i>

           <i class="material-icons"  :class="{ 'is-active': isActive.strike() }"
          @click="commands.strike">format_strikethrough</i>

            <i class="material-icons"  :class="{ 'is-active': isActive.underline() }"
          @click="commands.underline">format_underlined</i>

           <i class="material-icons"  :class="{ 'is-active': isActive.paragraph() }"
          @click="commands.paragraph">local_parking</i>
          
           <i class="material-icons"  :class="{ 'is-active': isActive.horizontal_rule() }"
          @click="commands.horizontal_rule">minimize</i>
          
          <i class="material-icons" :class="{ 'is-active': isActive.heading({ level: 4 }) }"
          @click="commands.heading({ level: 4 })">title</i>

          <i class="material-icons"  :class="{ 'is-active': isActive.ordered_list() }"
          @click="commands.ordered_list">list</i>
      
          <i class="material-icons menubar__button" :class="{ 'is-active': isActive.code_block() }" 
          @click="commands.code_block">code</i>
         

      </div>
    </editor-menu-bar>

    <editor-content class="editor__content" :editor="editor"  style="border:1px solid silver" />
  </div>
</template>

<script>
import Icon from '@/components/utilities/icons';
import { Editor, EditorContent, EditorMenuBar, EditorMenuBubble } from 'tiptap'
import {
  Blockquote,
  CodeBlock,
  HardBreak,
  Heading,
  HorizontalRule,
  OrderedList,
  BulletList,
  ListItem,
  TodoItem,
  TodoList,
  Bold,
  Code,
  Italic,
  Link,
  Strike,
  Underline,
  History,
} from 'tiptap-extensions'

export default {
  components: {
    name: "tiptap",
    EditorContent,
    EditorMenuBar,
    EditorMenuBubble
    
  },
  props:['editorContent'],
  data() {
    return {
      
      editor: new Editor({
         onUpdate: ({ getHTML }) => {         
         this.$store.commit('setApartmentComment', getHTML())
         console.log(this.$store.state.apartmentComment)
      
    },
        extensions: [
          new Blockquote(),
          new BulletList(),
          new CodeBlock(),
          new HardBreak(),
          new Heading({ levels: [1, 2, 3] }),
          new HorizontalRule(),
          new ListItem(),
          new OrderedList(),
          new TodoItem(),
          new TodoList(),
          new Link(),
          new Bold(),
          new Code(),
          new Italic(),
          new Strike(),
          new Underline(),
          new History(),
        ],
        content: this.$store.state.apartmentComment,
      }),
    }
   
  },
  mounted(){
     // this.$emit('sendContent', this.editor.getHTML() );  

      // I get the html using property 'editorContent' and send using vuex store
      this.editor.setContent(this.$store.state.apartmentComment);        
  },
  
  beforeDestroy() {
      this.editor.setContent(this.$store.state.apartmentComment);  
    this.editor.destroy()
  },
  
}
</script>