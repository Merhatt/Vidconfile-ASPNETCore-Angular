import { Component, OnInit, Input } from '@angular/core';
import { VideoComment } from '../models/video-comment';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit {
  @Input() videoComment: VideoComment;

  constructor() { }

  ngOnInit() {
  }
}
