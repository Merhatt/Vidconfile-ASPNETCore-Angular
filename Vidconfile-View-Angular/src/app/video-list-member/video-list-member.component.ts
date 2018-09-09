import { Component, OnInit, Input } from '@angular/core';
import { Video } from '../models/video';

@Component({
  selector: 'app-video-list-member',
  templateUrl: './video-list-member.component.html',
  styleUrls: ['./video-list-member.component.css']
})
export class VideoListMemberComponent implements OnInit {
  @Input() video: Video;

  constructor() { }

  ngOnInit() {
  }

}
